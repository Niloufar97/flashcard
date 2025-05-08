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
        private readonly string _iconfinderApiKey;

        public flashcardGeneratorService( AppDbContext context)
        {
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")!;
            _client = new ChatClient(model: "gpt-4", apiKey: _apiKey);
            _dbContext = context;
            _iconfinderApiKey = Environment.GetEnvironmentVariable("ICONFINDER_API_KEY")!;
        }

        private async Task<string?> GetIconUrlAsync(string query)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_iconfinderApiKey}");

            // This is the correct way to call the API (no invalid query params)
            var response = await httpClient.GetAsync($"https://api.iconfinder.com/v4/icons/search?query={Uri.EscapeDataString(query)}&count=1&premium=0&vector=0");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            try
            {
                var icon = doc.RootElement.GetProperty("icons")[0];

                // Get raster_sizes array
                var rasterSizes = icon.GetProperty("raster_sizes").EnumerateArray();

                var size512 = rasterSizes.FirstOrDefault(s => s.GetProperty("size").GetInt32() == 512);

                if (size512.ValueKind == JsonValueKind.Undefined)
                {
                    // fallback to largest available size
                    size512 = rasterSizes.LastOrDefault();
                }

                var previewUrl = size512
                    .GetProperty("formats")[0]
                    .GetProperty("preview_url")
                    .GetString();

                return previewUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to parse icon JSON: " + ex.Message);
                return null;
            }
        }

        public async Task<GeneratedFlashcardsResponseDto> GetFlashcardsAsync(string topic, DifficultyLevel level)
        {
            var prompt0 = "[mandetory] Give me only the raw JSON array of English and Danish flashcards. Do not include any explanation, formatting, or code block markdown. I want only the JSON output in this format {{\r\n    \"English\": \"cat\",\r\n    \"Danish\": \"kat\"\r\n  },\r\n  ...}";
            var prompt1 = $"Generate 5 flashcards for the topic '{topic}', where each flashcard consists of an English word and its Danish translation. The difficulty level should be {level.ToString().ToLower()} (e.g., easier words for 'easy', more complex or domain-specific vocabulary for 'hard').";

            // Send the prompt to OpenAI and get the response
            ChatCompletion completion = await _client.CompleteChatAsync([prompt0, prompt1]);

            // Access the first message's content and return it as a string
            var jsonText = completion.Content[0].Text;
            var GeneratedFlashcards = JsonSerializer.Deserialize<FlashcardDto[]>(jsonText);

            // Save new topic and flashcards to database
            var topicIconUrl = await GetIconUrlAsync(topic);
            var topicEntity = new Topic
            {
                Name = topic,
                Level = level,
                IconUrl = topicIconUrl
            };
            _dbContext.Topics.Add(topicEntity);
            await _dbContext.SaveChangesAsync();

            foreach (var flashcard in GeneratedFlashcards)
            {
                var iconUrl = await GetIconUrlAsync(flashcard.English);
                var flashcardEntity = new Flashcard
                {
                    EnglishWord = flashcard.English,
                    DanishWord = flashcard.Danish,
                    TopicId = topicEntity.TopicId,
                    Topic = topicEntity,
                    IconUrl = iconUrl
                };
                _dbContext.Flashcards.Add(flashcardEntity);
            }
            await _dbContext.SaveChangesAsync();

            var result = new GeneratedFlashcardsResponseDto() { TopicName = topic, Flashcards = GeneratedFlashcards };
            return result;
        }
    }
}


