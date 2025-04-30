using flashcard.Data;
using flashcard.DTOs;
using flashcard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Humanizer;
using flashcard.Models;
using Microsoft.AspNetCore.Http.HttpResults;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<flashcardGeneratorService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowViteFrontend");

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

app.MapGet("/topic", async(AppDbContext context) => { 
    var topics = await context.Topics
        .Select(t => new TopicDto
        {
            Id = t.TopicId,
            TopicName = t.Name,
            FlashcardCount = t.Flashcards.Count()  
        })
        .ToListAsync();

    return Results.Ok(topics);
});

app.MapGet("/topic/{id}/flashcards", async (AppDbContext context, int id) => {

    var flashcards = await context.Flashcards
                                  .Where(f => f.TopicId == id)
                                  .Select(f => new FlashcardDto
                                  {
                                      English = f.EnglishWord,
                                      Danish = f.DanishWord
                                  })
                                  .ToListAsync();
    return Results.Ok(flashcards);
});

app.MapPost("/generate-flashcards" , async (flashcardGeneratorService generatorService, AppDbContext context, [FromBody] TopicRequestDto requestDto) =>
{

    //topic is required
    if (string.IsNullOrEmpty(requestDto.TopicName))
    {
        return Results.BadRequest("Topic is required.");
    }

    // Check if the topic already exists in the database
    var existingTopic = await context.Topics.FirstOrDefaultAsync(t => t.Name.ToLower() == requestDto.TopicName.ToLower());

    if(existingTopic != null)
    {
        return Results.BadRequest("Topic already exists.");
    }

    // Call the service to get flashcards (raw response from OpenAI)
    var result = await generatorService.GetFlashcardsAsync(requestDto.TopicName);
    return Results.Ok(result);
});

app.Run();
