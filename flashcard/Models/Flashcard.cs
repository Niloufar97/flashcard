using System.Text.Json.Serialization;

namespace flashcard.Models
{
    public class Flashcard
    {
        public int Id { get; set; }

        public int TopicId { get; set; }
        public required Topic Topic { get; set; }
        public required string EnglishWord { get; set; }
        public required string DanishWord { get; set; }
        public string? IconUrl { get; set; }

    }
}
