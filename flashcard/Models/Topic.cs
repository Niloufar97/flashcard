namespace flashcard.Models
{
    public class Topic
    {
        public enum DifficultyLevel
        {
            Easy = 1,
            Medium = 2,
            Hard = 3
        }
        public int TopicId { get; set; }
        public string? Name { get; set; }
        public string? IconUrl { get; set; }
        public DifficultyLevel Level { get; set; } = DifficultyLevel.Easy;
        public  List<Flashcard> Flashcards{ get; set; } = new List<Flashcard>();

    }
}
