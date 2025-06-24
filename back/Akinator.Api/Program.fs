open System
open System.Net.Http
open System.Text
open System.Text.Json
open System.Text.Encodings.Web
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open TaskBuilder
open Microsoft.AspNetCore.Http
open Akinator.Database

type AnswerItem = {
    question: string
    answer: string
}

type AnswerRequest = {
    answers: AnswerItem list
}

// Добавляем этот тип для ответа от Prolog
type PrologResponse = {
    result: string[]
}

let prologUrl = "http://localhost:6000"
let httpClient = new HttpClient()

let normalizeAnswer (ans: string) =
    match ans.ToLower() with
    | "yes" -> "Да"
    | "no" -> "Нет"
    | other -> other

let normalizeAnswers (answers: AnswerItem list) =
    answers |> List.map (fun a -> { a with answer = normalizeAnswer a.answer })

let postToProlog (answers: AnswerItem list) = task {
    let normalizedAnswers = normalizeAnswers answers
    let options = JsonSerializerOptions(Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping)
    
    let requestData = { answers = normalizedAnswers }
    let requestBody = JsonSerializer.Serialize(requestData, options)
    
    printfn "Отправляем в пролог: %s" requestBody
    use content = new StringContent(requestBody, Encoding.UTF8, "application/json")

    try
        let! response = httpClient.PostAsync(prologUrl + "/akinator", content)
        response.EnsureSuccessStatusCode() |> ignore
        let! responseBody = response.Content.ReadAsStringAsync()
        printfn "Ответ от пролога: %s" responseBody

        let responseOptions = JsonSerializerOptions(PropertyNameCaseInsensitive = true)
        let prologResponse = JsonSerializer.Deserialize<PrologResponse>(responseBody, responseOptions)
                
        match prologResponse.result with
        | [||] -> 
            printfn "Массив result пуст"
            return "{\"not_found\":\"empty\"}"
        | [|singleResult|] -> 
            printfn "Получен 1 результат: %s" singleResult
            return responseBody
        | results when results.Length > 1 -> 
            printfn "Получено %d результатов, запрашиваем следующий вопрос" results.Length
            let askedQuestions = normalizedAnswers |> List.map (fun a -> a.question)
            let nextQuestionRequest = {| asked_questions = askedQuestions |}
            let nextQuestionBody = JsonSerializer.Serialize(nextQuestionRequest, options)
            use nextQuestionContent = new StringContent(nextQuestionBody, Encoding.UTF8, "application/json")
            let! nextQuestionResponse = httpClient.PostAsync(prologUrl + "/next_question", nextQuestionContent)
            let! nextQuestionBody = nextQuestionResponse.Content.ReadAsStringAsync()
            printfn "Получен следующий вопрос: %s" nextQuestionBody
            return nextQuestionBody
        | _ -> 
            return responseBody
    with ex ->
        printfn "Ошибка при запросе к прологу: %s" ex.Message
        return "{\"result\":\"error\"}"
}


let postAkinatorHandler : HttpHandler =
    fun next ctx -> task {
        let! answerRequest = ctx.BindJsonAsync<AnswerRequest>()
        printfn "Получены ответы от фронта: %A" answerRequest.answers
        let! responseBody = postToProlog answerRequest.answers
        ctx.SetContentType "application/json"
        return! ctx.WriteStringAsync(responseBody)
    }

let getFirstQuestionHandler : HttpHandler =
    fun next ctx -> task {
        let! responseBody = httpClient.GetStringAsync(prologUrl + "/first_question")
        ctx.SetContentType "application/json"
        return! ctx.WriteStringAsync(responseBody)
    }

let getAllFactsHandler : HttpHandler =
    fun next ctx -> task {
        let dbContext = ctx.GetService<AkinatorContext>()
        let facts = DataAccess.getAnswersForAllPlayersJson dbContext
        return! json facts next ctx
    }

let webApp =
    choose [
        route "/first_question" >=> GET >=> getFirstQuestionHandler
        route "/akinator" >=> POST >=> postAkinatorHandler
        route "/facts" >=> GET >=> getAllFactsHandler
        setStatusCode 404 >=> text "Not Found"
    ]


let configureServices (services : IServiceCollection) =
    services.AddDbContext<AkinatorContext>() |> ignore
    services.AddCors(fun options ->
        options.AddPolicy("AllowLocalhost3000", fun builder ->
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   |> ignore)
    ) |> ignore
    services.AddGiraffe() |> ignore

let configureApp (app : IApplicationBuilder) =
    app.UseCors("AllowLocalhost3000") |> ignore
    app.UseGiraffe webApp

[<EntryPoint>]
let main _ =
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder -> 
            webHostBuilder
                .Configure(configureApp)
                .ConfigureServices(configureServices)
                |> ignore)
        .Build()
        .Run()
    0
