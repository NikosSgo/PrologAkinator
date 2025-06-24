namespace Akinator.Database

open System
open System.Linq
open Microsoft.EntityFrameworkCore

module DataAccess =

    // CRUD операции для Player
    let addPlayer (context: AkinatorContext) (name: string) =
        try
            let player = Player(Name = name)
            context.Players.Add(player) |> ignore
            context.SaveChanges() |> ignore
            player
        with
        | ex -> 
            printfn "Ошибка при добавлении игрока: %s" ex.Message
            reraise()

    let getPlayer (context: AkinatorContext) (id: int) =
        try
            context.Players.Find(id)
        with
        | ex -> 
            printfn "Ошибка при получении игрока: %s" ex.Message
            reraise()

    let updatePlayer (context: AkinatorContext) (player: Player) =
        try
            context.Players.Update(player) |> ignore
            context.SaveChanges() |> ignore
            player
        with
        | ex -> 
            printfn "Ошибка при обновлении игрока: %s" ex.Message
            reraise()

    let deletePlayer (context: AkinatorContext) (id: int) =
        try
            let player = context.Players.Find(id)
            if not (isNull player) then
                context.Players.Remove(player) |> ignore
                context.SaveChanges() |> ignore
                true
            else
                false
        with
        | ex -> 
            printfn "Ошибка при удалении игрока: %s" ex.Message
            reraise()

    // CRUD операции для Question
    let addQuestion (context: AkinatorContext) (text: string) =
        try
            let question = Question(Text = text)
            context.Questions.Add(question) |> ignore
            context.SaveChanges() |> ignore
            question
        with
        | ex -> 
            printfn "Ошибка при добавлении вопроса: %s" ex.Message
            reraise()

    let getQuestion (context: AkinatorContext) (id: int) =
        try
            context.Questions.Find(id)
        with
        | ex -> 
            printfn "Ошибка при получении вопроса: %s" ex.Message
            reraise()

    let updateQuestion (context: AkinatorContext) (question: Question) =
        try
            context.Questions.Update(question) |> ignore
            context.SaveChanges() |> ignore
            question
        with
        | ex -> 
            printfn "Ошибка при обновлении вопроса: %s" ex.Message
            reraise()

    let deleteQuestion (context: AkinatorContext) (id: int) =
        try
            let question = context.Questions.Find(id)
            if not (isNull question) then
                context.Questions.Remove(question) |> ignore
                context.SaveChanges() |> ignore
                true
            else
                false
        with
        | ex -> 
            printfn "Ошибка при удалении вопроса: %s" ex.Message
            reraise()

    // CRUD операции для PlayerAnswer
    let addPlayerAnswer (context: AkinatorContext) (playerId: int) (questionId: int) (answer: int) =
        try
            let playerAnswer = PlayerAnswer(
                PlayerId = playerId,
                QuestionId = questionId,
                Answer = answer
            )
            context.PlayerAnswers.Add(playerAnswer) |> ignore
            context.SaveChanges() |> ignore
            playerAnswer
        with
        | ex -> 
            printfn "Ошибка при добавлении ответа игрока: %s" ex.Message
            reraise()

    let getPlayerAnswers (context: AkinatorContext) (playerId: int) =
        try
            context.PlayerAnswers
                .Where(fun pa -> pa.PlayerId = playerId)
                .Include(fun pa -> pa.Question)
                .ToList()
        with
        | ex -> 
            printfn "Ошибка при получении ответов игрока: %s" ex.Message
            reraise()

    let updatePlayerAnswer (context: AkinatorContext) (playerAnswer: PlayerAnswer) =
        try
            context.PlayerAnswers.Update(playerAnswer) |> ignore
            context.SaveChanges() |> ignore
            playerAnswer
        with
        | ex -> 
            printfn "Ошибка при обновлении ответа игрока: %s" ex.Message
            reraise()

    let deletePlayerAnswer (context: AkinatorContext) (id: int) =
        try
            let playerAnswer = context.PlayerAnswers.Find(id)
            if not (isNull playerAnswer) then
                context.PlayerAnswers.Remove(playerAnswer) |> ignore
                context.SaveChanges() |> ignore
                true
            else
                false
        with
        | ex -> 
            printfn "Ошибка при удалении ответа игрока: %s" ex.Message
            reraise()

    let initializationPlayers (context: AkinatorContext) =
        try
            // Добавляем 10 вопросов
            let fromSouthAmerica = addQuestion context "Игрок родом из Южной Америки?"
            let inTop5Leagues = addQuestion context "Игрок в настоящее время выступает в одной из топ-5 европейских лиг?"
            let isForward = addQuestion context "Игрок является нападающим или атакующим полузащитником?"
            let olderThan30 = addQuestion context "Игрок старше 30 лет?"
            let shorterThan180 = addQuestion context "Рост игрока меньше 180 см?"
            let isLeftFooted = addQuestion context "Игрок левша?"
            let hasBallonDor = addQuestion context "Игрок выигрывал Золотой мяч (Ballon d'Or)?"
            let over100Caps = addQuestion context "Игрок провёл более 100 матчей за сборную?"
            let wearsNumber10 = addQuestion context "Игрок обычно играет под номером 10?"
            let wonChampionsLeague = addQuestion context "Игрок выигрывал Лигу чемпионов?"

            // Южноамериканские нападающие (30)
            let messi = addPlayer context "Лионель Месси"
            let neymar = addPlayer context "Неймар"
            let aguero = addPlayer context "Серхио Агуэро"
            let jesus = addPlayer context "Габриэль Жезус"
            let vinicius = addPlayer context "Винисиус Жуниор"
            let rodrygo = addPlayer context "Родриго"
            let richarlison = addPlayer context "Ришарлисон"
            let suarez = addPlayer context "Луис Суарес"
            let cavani = addPlayer context "Эдинсон Кавани"
            let nunez = addPlayer context "Дарвин Нуньес"
            let antony = addPlayer context "Антони"
            let rafinha = addPlayer context "Рафинья"
            let alvarez = addPlayer context "Хулиан Альварес"
            let martinez = addPlayer context "Лаутаро Мартинес"
            let dybala = addPlayer context "Пауло Дибала"
            let diMaria = addPlayer context "Анхель Ди Мария"
            let coutinho = addPlayer context "Филиппе Коутиньо"
            let firmino = addPlayer context "Роберто Фирмино"
            let gabigol = addPlayer context "Габриэль Барбоза"
            let cunha = addPlayer context "Матеус Кунья"
            let jota = addPlayer context "Диого Жота"
            let moura = addPlayer context "Лукас Моура"
            let ribeiro = addPlayer context "Эвертон Рибейро"
            let jorginho = addPlayer context "Жоржиньо"
            let neres = addPlayer context "Давид Нерес"
            let castro = addPlayer context "Матеус Кастро"
            let martinelli = addPlayer context "Габриэль Мартинелли"
            let jimenez = addPlayer context "Рауль Хименес"
            let calleri = addPlayer context "Хонатан Каллери"
            let romero = addPlayer context "Серхио Ромеро"

            // Добавляем ответы для Месси (пример)
            addPlayerAnswer context messi.Id fromSouthAmerica.Id 1 |> ignore
            addPlayerAnswer context messi.Id inTop5Leagues.Id 0 |> ignore
            addPlayerAnswer context messi.Id isForward.Id 1 |> ignore
            addPlayerAnswer context messi.Id olderThan30.Id 1 |> ignore
            addPlayerAnswer context messi.Id shorterThan180.Id 1 |> ignore
            addPlayerAnswer context messi.Id isLeftFooted.Id 1 |> ignore
            addPlayerAnswer context messi.Id hasBallonDor.Id 1 |> ignore
            addPlayerAnswer context messi.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context messi.Id wearsNumber10.Id 1 |> ignore
            addPlayerAnswer context messi.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Неймара (пример)
            addPlayerAnswer context neymar.Id fromSouthAmerica.Id 1 |> ignore
            addPlayerAnswer context neymar.Id inTop5Leagues.Id 0 |> ignore
            addPlayerAnswer context neymar.Id isForward.Id 1 |> ignore
            addPlayerAnswer context neymar.Id olderThan30.Id 1 |> ignore
            addPlayerAnswer context neymar.Id shorterThan180.Id 1 |> ignore
            addPlayerAnswer context neymar.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context neymar.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context neymar.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context neymar.Id wearsNumber10.Id 1 |> ignore
            addPlayerAnswer context neymar.Id wonChampionsLeague.Id 1 |> ignore

            // Европейские нападающие (30)
            let ronaldo = addPlayer context "Криштиану Роналду"
            let mbappe = addPlayer context "Килиан Мбаппе"
            let benzema = addPlayer context "Карим Бензема"
            let lewandowski = addPlayer context "Роберт Левандовски"
            let haaland = addPlayer context "Эрлинг Холанд"
            let kane = addPlayer context "Гарри Кейн"
            let rashford = addPlayer context "Маркус Рэшфорд"
            let sancho = addPlayer context "Джейдон Санчо"
            let saka = addPlayer context "Букайо Сака"
            let foden = addPlayer context "Фил Фоден"
            let grealish = addPlayer context "Джек Грилиш"
            let sterling = addPlayer context "Рахим Стерлинг"
            let mount = addPlayer context "Мэйсон Маунт"
            let giroud = addPlayer context "Оливье Жиру"
            let griezmann = addPlayer context "Антуан Гризманн"
            let dembele = addPlayer context "Усман Дембеле"
            let coman = addPlayer context "Кингсли Коман"
            let nkunku = addPlayer context "Кристофер Нкунку"
            let koloMuani = addPlayer context "Рандаль Коло Муани"
            let osimhen = addPlayer context "Виктор Осимхен"
            let lukaku = addPlayer context "Ромелу Лукаку"
            let vlahovic = addPlayer context "Душеан Влахович"
            let mitrovic = addPlayer context "Александар Митрович"
            let morata = addPlayer context "Альваро Мората"
            let moreno = addPlayer context "Жерард Морено"
            let depay = addPlayer context "Мемфис Депай"
            let kimmich = addPlayer context "Йосуа Киммих"
            let werner = addPlayer context "Тимо Вернер"
            let schick = addPlayer context "Патрик Шик"
            let szoboszlai = addPlayer context "Доминик Собослаи"

            // Добавляем ответы для Роналду (пример)
            addPlayerAnswer context ronaldo.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context ronaldo.Id inTop5Leagues.Id 0 |> ignore
            addPlayerAnswer context ronaldo.Id isForward.Id 1 |> ignore
            addPlayerAnswer context ronaldo.Id olderThan30.Id 1 |> ignore
            addPlayerAnswer context ronaldo.Id shorterThan180.Id 0 |> ignore
            addPlayerAnswer context ronaldo.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context ronaldo.Id hasBallonDor.Id 1 |> ignore
            addPlayerAnswer context ronaldo.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context ronaldo.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context ronaldo.Id wonChampionsLeague.Id 1 |> ignore

            // Полузащитники (40)
            let deBruyne = addPlayer context "Кевин Де Брюйне"
            let modric = addPlayer context "Лука Модрич"
            let kroos = addPlayer context "Тони Кроос"
            let bellingham = addPlayer context "Джуд Беллингем"
            let bernardoSilva = addPlayer context "Бернарду Силва"
            let brunoFernandes = addPlayer context "Бруно Фернандеш"
            let odegaard = addPlayer context "Мартин Эдегор"
            let rice = addPlayer context "Деклан Райс"
            let deJong = addPlayer context "Френки де Йонг"
            let pedri = addPlayer context "Педри"
            let gavi = addPlayer context "Гави"
            let verratti = addPlayer context "Марко Верратти"
            let kante = addPlayer context "Н'Голо Канте"
            let pogba = addPlayer context "Поль Погба"
            let tchouameni = addPlayer context "Орельен Чуамени"
            let camavinga = addPlayer context "Эдуардо Камавинга"
            let rodri = addPlayer context "Родри"
            let thiago = addPlayer context "Тиаго Алькантара"
            let casemiro = addPlayer context "Каземиро"
            let fabinho = addPlayer context "Фабиньо"
            let alba = addPlayer context "Йорди Альба"
            let busquets = addPlayer context "Серхио Бускетс"
            let gundogan = addPlayer context "Илкай Гюндоган"
            let partey = addPlayer context "Томас Парти"
            let xhaka = addPlayer context "Гранит Джака"
            let eriksen = addPlayer context "Кристиан Эриксен"
            let maddison = addPlayer context "Джеймс Мэддисон"
            let tielemans = addPlayer context "Юри Тилеманс"
            let yazici = addPlayer context "Юсуф Языджи"
            let neves = addPlayer context "Рубен Невеш"
            let palhinha = addPlayer context "Жоау Пальинья"
            let kovacic = addPlayer context "Матео Ковачич"
            let brozovic = addPlayer context "Марсело Брозович"
            let barella = addPlayer context "Николо Барелла"
            let tonali = addPlayer context "Сандро Тонали"
            let locatelli = addPlayer context "Мануэль Локателли"
            let calhanoglu = addPlayer context "Хакан Чалханоглу"
            let milinkovicSavic = addPlayer context "Сергей Милинкович-Савич"
            let pellegrini = addPlayer context "Лоренцо Пеллегрини"
            let pellegri = addPlayer context "Пьетро Пелигри"

            // Добавляем ответы для Модрича (пример)
            addPlayerAnswer context modric.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context modric.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context modric.Id isForward.Id 0 |> ignore
            addPlayerAnswer context modric.Id olderThan30.Id 1 |> ignore
            addPlayerAnswer context modric.Id shorterThan180.Id 1 |> ignore
            addPlayerAnswer context modric.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context modric.Id hasBallonDor.Id 1 |> ignore
            addPlayerAnswer context modric.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context modric.Id wearsNumber10.Id 1 |> ignore
            addPlayerAnswer context modric.Id wonChampionsLeague.Id 1 |> ignore

            // Защитники (40)
            let vanDijk = addPlayer context "Вирджил ван Дейк"
            let rubenDias = addPlayer context "Рубен Диаш"
            let cancelo = addPlayer context "Жоау Канселу"
            let walker = addPlayer context "Каил Уокер"
            let alexanderArnold = addPlayer context "Трент Александер-Арнольд"
            let robertson = addPlayer context "Эндрю Робертсон"
            let davies = addPlayer context "Альфонсо Дэвис"
            let alaba = addPlayer context "Давид Алаба"
            let rudiger = addPlayer context "Антонио Рюдигер"
            let militao = addPlayer context "Эдер Милитан"
            let marquinhos = addPlayer context "Маркиньос"
            let thiagoSilva = addPlayer context "Тиаго Силва"
            let varane = addPlayer context "Рафаэль Варан"
            let kounde = addPlayer context "Жюль Кунде"
            let upamecano = addPlayer context "Дают Упамекано"
            let hakimi = addPlayer context "Ашраф Хакими"
            let theoHernandez = addPlayer context "Тео Эрнандес"
            let lucasHernandez = addPlayer context "Люка Эрнандес"
            let deLigt = addPlayer context "Маттейс де Лигт"
            let gvardiol = addPlayer context "Йошко Гвардиол"
            let stones = addPlayer context "Джон Стоунс"
            let laporte = addPlayer context "Аймерик Лапорт"
            let ake = addPlayer context "Натан Аке"
            let chilwell = addPlayer context "Бен Чилвелл"
            let pereira = addPlayer context "Рикардо Перейра"
            let dalot = addPlayer context "Диогу Далот"
            let wanBissaka = addPlayer context "Аарон Ван-Биссака"
            let lenglet = addPlayer context "Клеман Лангле"
            let kimpembe = addPlayer context "Пресель Кимпебе"
            let spinazzola = addPlayer context "Леонардо Спинаццола"
            let felix = addPlayer context "Жоау Феликс"
            let grimaldo = addPlayer context "Алехандро Гримальдо"
            let havertz = addPlayer context "Кай Хаверц"
            let mendes = addPlayer context "Нуну Мендеш"
            let dari = addPlayer context "Ашраф Дари"
            let milenkovic = addPlayer context "Никола Миленкович"
            let bremer = addPlayer context "Глебон Бремер"
            let dimarco = addPlayer context "Федерико Димарко"
            let diLorenzo = addPlayer context "Джованни Ди Лоренцо"
            let bastoni = addPlayer context "Алессандро Бастони"

            // Вратари (20)
            let courtois = addPlayer context "Тибо Куртуа"
            let alisson = addPlayer context "Алиссон Бекер"
            let ederson = addPlayer context "Эдерсон"
            let terStegen = addPlayer context "Марк-Андре тер Штеген"
            let oblak = addPlayer context "Ян Облак"
            let maignan = addPlayer context "Майк Меньян"
            let donnarumma = addPlayer context "Джанлуиджи Доннарумма"
            let emiMartinez = addPlayer context "Эмилиано Мартинес"
            let onana = addPlayer context "Андре Онана"
            let deGea = addPlayer context "Давид де Хеа"
            let kepa = addPlayer context "Кепа Аррисабалага"
            let lloris = addPlayer context "Уго Льорис"
            let gulacsi = addPlayer context "Петер Гулачи"
            let samuelUmtiti = addPlayer context "Самюэль Умтити"
            let sommer = addPlayer context "Ян Зомер"
            let szczesny = addPlayer context "Войцех Щенсный"
            let nuebel = addPlayer context "Александр Нюбель"
            let trapp = addPlayer context "Кевин Трапп"
            let bialkowski = addPlayer context "Бартош Бялковский"

            // Добавляем ответы для Куртуа (пример)
            addPlayerAnswer context courtois.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context courtois.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context courtois.Id isForward.Id 0 |> ignore
            addPlayerAnswer context courtois.Id olderThan30.Id 1 |> ignore
            addPlayerAnswer context courtois.Id shorterThan180.Id 0 |> ignore
            addPlayerAnswer context courtois.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context courtois.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context courtois.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context courtois.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context courtois.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Вирджила ван Дейка
            addPlayerAnswer context vanDijk.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context vanDijk.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context vanDijk.Id isForward.Id 0 |> ignore
            addPlayerAnswer context vanDijk.Id olderThan30.Id 1 |> ignore
            addPlayerAnswer context vanDijk.Id shorterThan180.Id 0 |> ignore
            addPlayerAnswer context vanDijk.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context vanDijk.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context vanDijk.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context vanDijk.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context vanDijk.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Рубена Диаша
            addPlayerAnswer context rubenDias.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context rubenDias.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context rubenDias.Id isForward.Id 0 |> ignore
            addPlayerAnswer context rubenDias.Id olderThan30.Id 0 |> ignore
            addPlayerAnswer context rubenDias.Id shorterThan180.Id 0 |> ignore
            addPlayerAnswer context rubenDias.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context rubenDias.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context rubenDias.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context rubenDias.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context rubenDias.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Жоау Канселу
            addPlayerAnswer context cancelo.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context cancelo.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context cancelo.Id isForward.Id 0 |> ignore
            addPlayerAnswer context cancelo.Id olderThan30.Id 0 |> ignore
            addPlayerAnswer context cancelo.Id shorterThan180.Id 0 |> ignore
            addPlayerAnswer context cancelo.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context cancelo.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context cancelo.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context cancelo.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context cancelo.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Кайла Уокера
            addPlayerAnswer context walker.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context walker.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context walker.Id isForward.Id 0 |> ignore
            addPlayerAnswer context walker.Id olderThan30.Id 1 |> ignore
            addPlayerAnswer context walker.Id shorterThan180.Id 0 |> ignore
            addPlayerAnswer context walker.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context walker.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context walker.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context walker.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context walker.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Трента Александер-Арнольда
            addPlayerAnswer context alexanderArnold.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context alexanderArnold.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context alexanderArnold.Id isForward.Id 0 |> ignore
            addPlayerAnswer context alexanderArnold.Id olderThan30.Id 0 |> ignore
            addPlayerAnswer context alexanderArnold.Id shorterThan180.Id 0 |> ignore
            addPlayerAnswer context alexanderArnold.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context alexanderArnold.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context alexanderArnold.Id over100Caps.Id 0 |> ignore
            addPlayerAnswer context alexanderArnold.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context alexanderArnold.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Эндрю Робертсона
            addPlayerAnswer context robertson.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context robertson.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context robertson.Id isForward.Id 0 |> ignore
            addPlayerAnswer context robertson.Id olderThan30.Id 0 |> ignore
            addPlayerAnswer context robertson.Id shorterThan180.Id 1 |> ignore
            addPlayerAnswer context robertson.Id isLeftFooted.Id 1 |> ignore
            addPlayerAnswer context robertson.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context robertson.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context robertson.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context robertson.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Альфонсо Дэвиса
            addPlayerAnswer context davies.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context davies.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context davies.Id isForward.Id 0 |> ignore
            addPlayerAnswer context davies.Id olderThan30.Id 0 |> ignore
            addPlayerAnswer context davies.Id shorterThan180.Id 0 |> ignore
            addPlayerAnswer context davies.Id isLeftFooted.Id 1 |> ignore
            addPlayerAnswer context davies.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context davies.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context davies.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context davies.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Давида Алабы
            addPlayerAnswer context alaba.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context alaba.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context alaba.Id isForward.Id 0 |> ignore
            addPlayerAnswer context alaba.Id olderThan30.Id 1 |> ignore
            addPlayerAnswer context alaba.Id shorterThan180.Id 1 |> ignore
            addPlayerAnswer context alaba.Id isLeftFooted.Id 1 |> ignore
            addPlayerAnswer context alaba.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context alaba.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context alaba.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context alaba.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Антонио Рюдигера
            addPlayerAnswer context rudiger.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context rudiger.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context rudiger.Id isForward.Id 0 |> ignore
            addPlayerAnswer context rudiger.Id olderThan30.Id 1 |> ignore
            addPlayerAnswer context rudiger.Id shorterThan180.Id 0 |> ignore
            addPlayerAnswer context rudiger.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context rudiger.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context rudiger.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context rudiger.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context rudiger.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Эдера Милитана
            addPlayerAnswer context militao.Id fromSouthAmerica.Id 1 |> ignore
            addPlayerAnswer context militao.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context militao.Id isForward.Id 0 |> ignore
            addPlayerAnswer context militao.Id olderThan30.Id 0 |> ignore
            addPlayerAnswer context militao.Id shorterThan180.Id 0 |> ignore
            addPlayerAnswer context militao.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context militao.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context militao.Id over100Caps.Id 0 |> ignore
            addPlayerAnswer context militao.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context militao.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Маркиньоса
            addPlayerAnswer context marquinhos.Id fromSouthAmerica.Id 1 |> ignore
            addPlayerAnswer context marquinhos.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context marquinhos.Id isForward.Id 0 |> ignore
            addPlayerAnswer context marquinhos.Id olderThan30.Id 0 |> ignore
            addPlayerAnswer context marquinhos.Id shorterThan180.Id 0 |> ignore
            addPlayerAnswer context marquinhos.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context marquinhos.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context marquinhos.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context marquinhos.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context marquinhos.Id wonChampionsLeague.Id 0 |> ignore

            // Добавляем ответы для Тиаго Силвы
            addPlayerAnswer context thiagoSilva.Id fromSouthAmerica.Id 1 |> ignore
            addPlayerAnswer context thiagoSilva.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context thiagoSilva.Id isForward.Id 0 |> ignore
            addPlayerAnswer context thiagoSilva.Id olderThan30.Id 1 |> ignore
            addPlayerAnswer context thiagoSilva.Id shorterThan180.Id 0 |> ignore
            addPlayerAnswer context thiagoSilva.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context thiagoSilva.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context thiagoSilva.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context thiagoSilva.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context thiagoSilva.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Рафаэля Варана
            addPlayerAnswer context varane.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context varane.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context varane.Id isForward.Id 0 |> ignore
            addPlayerAnswer context varane.Id olderThan30.Id 1 |> ignore
            addPlayerAnswer context varane.Id shorterThan180.Id 0 |> ignore
            addPlayerAnswer context varane.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context varane.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context varane.Id over100Caps.Id 1 |> ignore
            addPlayerAnswer context varane.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context varane.Id wonChampionsLeague.Id 1 |> ignore

            // Добавляем ответы для Жюля Кунде
            addPlayerAnswer context kounde.Id fromSouthAmerica.Id 0 |> ignore
            addPlayerAnswer context kounde.Id inTop5Leagues.Id 1 |> ignore
            addPlayerAnswer context kounde.Id isForward.Id 0 |> ignore
            addPlayerAnswer context kounde.Id olderThan30.Id 0 |> ignore
            addPlayerAnswer context kounde.Id shorterThan180.Id 1 |> ignore
            addPlayerAnswer context kounde.Id isLeftFooted.Id 0 |> ignore
            addPlayerAnswer context kounde.Id hasBallonDor.Id 0 |> ignore
            addPlayerAnswer context kounde.Id over100Caps.Id 0 |> ignore
            addPlayerAnswer context kounde.Id wearsNumber10.Id 0 |> ignore
            addPlayerAnswer context kounde.Id wonChampionsLeague.Id 0 |> ignore

            printfn "База данных успешно инициализирована с 150 игроками и 10 вопросами"
        with
        | ex -> printfn "Ошибка при инициализации базы данных: %s" ex.Message

    let initializeDatabase (context: AkinatorContext) =
        try
            let created = context.Database.EnsureCreated()

            match created || not (context.Players.Any()) with
            | true -> initializationPlayers context
            | false -> printfn "Данные уже инициализированы — пропускаем."
        with
        | ex -> 
            printfn "Ошибка при инициализации базы данных: %s" ex.Message
            reraise()

    let getAnswersForAllPlayersJson (context: AkinatorContext) =
        try
            // Получаем все ответы игроков с включением вопроса и игрока
            let result = 
                context.PlayerAnswers
                    .Include(fun pa -> pa.Player)   // Включаем игрока
                    .Include(fun pa -> pa.Question) // Включаем вопрос
                    .ToList()                      // Получаем список ответов

            // Преобразуем в нужный формат для JSON
            result
            |> Seq.map (fun playerAnswer ->
    
                {| 
                    object = playerAnswer.Player.Name
                    question = playerAnswer.Question.Text
                    answer = playerAnswer.Answer
                |})
            |> List.ofSeq
        with
        | ex -> 
            printfn "Ошибка при получении данных: %s" ex.Message
            reraise()

    