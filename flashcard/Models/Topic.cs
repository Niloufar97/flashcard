namespace flashcard.Models
{
    public class Topic
    {
        public int TopicId { get; set; }

        public string Name { get; set; } 

        public DateTime CreatedAt { get; set; } 
        public  List<Flashcard> Flashcards{ get; set; } = new List<Flashcard>();

    }
}
