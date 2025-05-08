using flashcard.Data;
using flashcard.DTOs;
using flashcard.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace flashcard.Endpoints
{
    public static class TopicEndpoints
    {
        private static object generatorService;

        public static void  MapTopicEndpoints(this IEndpointRouteBuilder app) {
            // Endpoint to get all topics
            app.MapGet("/topic", async (AppDbContext context) =>
            {
                var topics = await context.Topics
                                            .Select(t => new TopicDto
                                            {
                                                Id = t.TopicId,
                                                TopicName = t.Name,
                                                Level = t.Level,
                                                IconUrl = t.IconUrl,
                                                FlashcardCount = t.Flashcards.Count()
                                            }
              ).ToListAsync();
                return Results.Ok(topics);
            });

            //Endpoint to get a specific topic's flashcards
            app.MapGet("/topic/{id}/flashcards" , async(AppDbContext context, int id) =>
            {
               var topic = await context.Topics
                                        .Where(t => t.TopicId == id)
                                        .Select(t =>t.Name)
                                        .FirstOrDefaultAsync();
                if (topic == null) return Results.NotFound();
                var flashcards = await context.Flashcards
                                              .Where(f => f.TopicId == id)
                                              .Select(f => new FlashcardDto
                                              {
                                                  English = f.EnglishWord,
                                                  Danish = f.DanishWord,
                                                  IconUrl = f.IconUrl
                                              })
                                              .ToListAsync();
                var result = new
                {
                    topic,
                    flashcards
                };
                return Results.Ok(result);
            });

            app.MapPost("topic/create", async (flashcardGeneratorService generator , AppDbContext context, [FromBody] TopicRequestDto requestDto) => { 
                if(string.IsNullOrEmpty(requestDto.TopicName))
                    return Results.BadRequest("Topic is required");

                var exists = await context.Topics
                                          .AnyAsync(t => t.Name.ToLower() == requestDto.TopicName.ToLower() && t.Level == requestDto.Level);

                if (exists)
                {
                    return Results.BadRequest("Topic with the same name and level already exists.");
                }

                var result = await generator.GetFlashcardsAsync(requestDto.TopicName, requestDto.Level);
                return Results.Ok(result);
            });
        }
    }
}