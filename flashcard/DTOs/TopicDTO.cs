namespace flashcard.DTOs
{
    public class TopicDto
    {
        public TopicDto(string topic)
        {
            TopicName = topic;
        }

        public string TopicName { get; set; }
        public FlashcardDto[] Flashcards { get; set; }
    }
}
