namespace flashcard.Models
{
    public class Flashcard
    {
        public int Id { get; set; }
        
        public int TopicId { get; set; }
        public string EnglishWord { get; set; } 

        public string DanishWord { get; set; }

        public Topic Topic { get; set; } 
    }
}
