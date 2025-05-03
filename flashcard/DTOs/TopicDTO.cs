using static flashcard.Models.Topic;

namespace flashcard.DTOs
{
    public class TopicDto
    {
        public int Id { get; set; }
        public string TopicName { get; set; }
        public DifficultyLevel Level { get; set; }
        public int FlashcardCount { get; set; }
        public string IconUrl { get; set; }
    }
}
