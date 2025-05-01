using flashcard.Models;
using flashcard.DTOs;
using OpenAI.Chat;
using System.Text.Json;
using flashcard.Data;
using static flashcard.Models.Topic;


namespace flashcard.Services
    
{
    public class flashcardGeneratorService
    {
        private readonly string _apiKey;
        private readonly ChatClient _client;
        private readonly AppDbContext _dbContext;

        public flashcardGeneratorService( AppDbContext context)
        {
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")!;
            _client = new ChatClient(model: "gpt-4", apiKey: _apiKey);
            _dbContext = context;
        }

        public async Task<GeneratedFlashcardsResponseDto> GetFlashcardsAsync(string topic, DifficultyLevel level)
        {
            var prompt0 = "[mandetory] Give me only the raw JSON array of English and Danish flashcards. Do not include any explanation, formatting, or code block markdown. I want only the JSON output in this format {{\r\n    \"English\": \"cat\",\r\n    \"Danish\": \"kat\"\r\n  },\r\n  ...}";
            var prompt1 = $"Generate 8 flashcards for the topic '{topic}', where each flashcard consists of an English word and its Danish translation. The difficulty level should be {level.ToString().ToLower()} (e.g., easier words for 'easy', more complex or domain-specific vocabulary for 'hard').";

            // Send the prompt to OpenAI and get the response
            ChatCompletion completion = await _client.CompleteChatAsync([prompt0, prompt1]);

            // Access the first message's content and return it as a string
            var jsonText = completion.Content[0].Text;
            var GeneratedFlashcards = JsonSerializer.Deserialize<FlashcardDto[]>(jsonText);

            // Save new topic and flashcards to database
            var topicEntity = new Topic
            {
                Name = topic,
                Level = level
            };
            _dbContext.Topics.Add(topicEntity);
            await _dbContext.SaveChangesAsync();

            foreach (var flashcard in GeneratedFlashcards)
            {
                var flashcardEntity = new Flashcard
                {
                    EnglishWord = flashcard.English,
                    DanishWord = flashcard.Danish,
                    TopicId = topicEntity.TopicId,
                    Topic = topicEntity
                };
                _dbContext.Flashcards.Add(flashcardEntity);
            }
            await _dbContext.SaveChangesAsync();

            var result = new GeneratedFlashcardsResponseDto() { TopicName = topic, Flashcards = GeneratedFlashcards };
            return result;
        }
    }
}


