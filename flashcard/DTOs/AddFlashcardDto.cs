namespace flashcard.DTOs
{
    public class AddFlashcardDto
    {
        public required string English { get; set; }
        public required string Danish { get; set; }
        public string? IconUrl { get; set; }

    }
}
