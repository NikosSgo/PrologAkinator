open Akinator.Database
open System

[<EntryPoint>]
let main argv =
    try
        use context = new AkinatorContext()
        
        // Инициализируем базу данных
        DataAccess.initializeDatabase context
        
        0
    with
    | ex ->
        printfn "Произошла ошибка: %s" ex.Message
        printfn "Stack trace: %s" ex.StackTrace
        1