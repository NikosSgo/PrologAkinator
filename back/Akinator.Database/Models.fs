namespace Akinator.Database

open System.ComponentModel.DataAnnotations

[<AllowNullLiteral>]
type Player() =
    [<Key>]
    member val Id = 0 with get, set
    
    [<Required>]
    [<StringLength(100)>]
    member val Name = "" with get, set
    
    member val PlayerAnswers = ResizeArray<PlayerAnswer>() with get, set

and [<AllowNullLiteral>] Question() =
    [<Key>]
    member val Id = 0 with get, set
    
    [<Required>]
    [<StringLength(500)>]
    member val Text = "" with get, set
    
    member val PlayerAnswers = ResizeArray<PlayerAnswer>() with get, set

and [<AllowNullLiteral>] PlayerAnswer() =
    [<Key>]
    member val Id = 0 with get, set
    
    [<Required>]
    member val PlayerId = 0 with get, set
    
    [<Required>]
    member val QuestionId = 0 with get, set
    
    [<Required>]
    [<Range(0, 1)>]
    member val Answer = 0 with get, set

    member val Player = Unchecked.defaultof<Player> with get, set
    member val Question = Unchecked.defaultof<Question> with get, set
