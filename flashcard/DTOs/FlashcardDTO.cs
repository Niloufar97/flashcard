using System.Text.Json.Serialization;

namespace flashcard.DTOs
{
    public class FlashcardDto
    {
        public required string English { get; set; }
        public required string Danish { get; set; }
        public required string IconUrl { get; set; }
    }
}
