using flashcard.Data;
using flashcard.DTOs;
using flashcard.Models;

namespace flashcard.Endpoints
{
    public static class FlashcardEndpoints
    {
        public static void MapFlashcardEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("flashcard/{TopicId}", async (AppDbContext context,int TopicId, AddFlashcardDto flashcard) => {
                var topic = await context.Topics.FindAsync(TopicId);
                if (topic == null)
                    return Results.NotFound("Topic not found");

                var newFlashcard = new Flashcard
                {
                    DanishWord = flashcard.Danish,
                    EnglishWord = flashcard.English,
                    IconUrl = flashcard.IconUrl,
                    TopicId = TopicId

                };
                await context.Flashcards.AddAsync(newFlashcard);
                await context.SaveChangesAsync();
                return Results.Created();
            });
        }
    }
}
