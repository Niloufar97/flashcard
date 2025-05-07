using flashcard.Data;
using flashcard.DTOs;
using Microsoft.EntityFrameworkCore;

namespace flashcard.Endpoints
{
    public static class TopicEndpints
    {
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
                                        .Select(t => new 
                                        {
                                            TopicName = t.Name,
                                        })
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
        }
    }
}
