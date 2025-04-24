using flashcard.Data;
using flashcard.DTOs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/flashcard", async (AppDbContext context) => { 
    var flashcards = await context.Flashcards
        .Include(f => f.Topic)
        .Select(f => new FlashcardDTO
        {
            EnglishWord = f.EnglishWord,
            DanishWord = f.DanishWord,
            Topic = f.Topic.Name
        })
        .ToListAsync();

    return Results.Ok(flashcards);
});


app.Run();
