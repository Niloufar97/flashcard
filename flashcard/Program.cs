using flashcard.Data;
using flashcard.DTOs;
using flashcard.Services;
using Microsoft.EntityFrameworkCore;
using OpenAI.Chat;
using System.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<flashcardGeneratorService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

//app.MapGet("/flashcard", async (AppDbContext context) => { 
//    var flashcards = await context.Flashcards
//        .Select(f => new FlashcardDto
//        {
//            EnglishWord = f.EnglishWord,
//            DanishWord = f.DanishWord,
//        })
//        .ToListAsync();

//    return Results.Ok(flashcards);
//});

app.MapGet("/generate-flashcards", async (flashcardGeneratorService generatorService) =>
{
    // Use the hardcoded topic 'animals' for now
    string topic = "animals";

    // Call the service to get flashcards (raw response from OpenAI)
    var result = await generatorService.GetFlashcardsAsync(topic);

    // Return the raw response as a simple text result (you could also return it as JSON)
    return Results.Ok(result);
});


app.Run();
