using System.Text.Json.Serialization;

namespace flashcard.Models
{
    public class Flashcard
    {
        public int Id { get; set; }

        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public string EnglishWord { get; set; }
        public string DanishWord { get; set; }

    }
}
