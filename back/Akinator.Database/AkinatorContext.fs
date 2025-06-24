namespace Akinator.Database

open Microsoft.EntityFrameworkCore
open DotNetEnv
open System
open System.Linq.Expressions
open System.Collections.Generic

type AkinatorContext() =
    inherit DbContext()

    [<DefaultValue>]
    val mutable players : DbSet<Player>
    member this.Players with get() = this.players and set v = this.players <- v

    [<DefaultValue>]
    val mutable questions : DbSet<Question>
    member this.Questions with get() = this.questions and set v = this.questions <- v

    [<DefaultValue>]
    val mutable playerAnswers : DbSet<PlayerAnswer>
    member this.PlayerAnswers with get() = this.playerAnswers and set v = this.playerAnswers <- v

    override this.OnConfiguring(optionsBuilder: DbContextOptionsBuilder) =
        try
            let envPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, ".env")
            DotNetEnv.Env.Load(envPath) |> ignore
            let connStr = Environment.GetEnvironmentVariable("CONNECTION_STRING")
            if String.IsNullOrEmpty(connStr) then
                failwith "CONNECTION_STRING environment variable is not set"
            optionsBuilder.UseNpgsql(connStr) |> ignore
        with
        | ex -> 
            printfn "Error loading environment variables: %s" ex.Message
            reraise()

    override this.OnModelCreating(modelBuilder: ModelBuilder) =
        modelBuilder.Entity<PlayerAnswer>()
            .HasOne(fun pa -> pa.Player)
            .WithMany("PlayerAnswers")
            .HasForeignKey([| "PlayerId" |])
            |> ignore

        modelBuilder.Entity<PlayerAnswer>()
            .HasOne(fun pa -> pa.Question)
            .WithMany("PlayerAnswers")
            .HasForeignKey([| "QuestionId" |])
            |> ignore

        // Индексы для оптимизации
        modelBuilder.Entity<Player>()
            .HasIndex([| "Name" |])
            |> ignore

        modelBuilder.Entity<Question>()
            .HasIndex([| "Text" |])
            |> ignore

        modelBuilder.Entity<PlayerAnswer>()
            .HasIndex([| "PlayerId" |])
            |> ignore

        modelBuilder.Entity<PlayerAnswer>()
            .HasIndex([| "QuestionId" |])
            |> ignore
