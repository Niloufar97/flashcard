using static flashcard.Models.Topic;

namespace flashcard.DTOs
{
    public class TopicRequestDto
    {
        public string TopicName { get; set; } = string.Empty;
        public DifficultyLevel Level { get; set; } = DifficultyLevel.Easy;
    }
}
