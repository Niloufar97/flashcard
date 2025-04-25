using flashcard.Models;
using flashcard.DTOs;
using OpenAI.Chat;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using System.Text.Json;


namespace flashcard.Services
    
{
    public class flashcardGeneratorService
    {
        private readonly string _apiKey;
        private readonly ChatClient _client;

        public flashcardGeneratorService(IConfiguration config)
        {
            _apiKey = config["OpenAI:ApiKey"]!;
            _client = new ChatClient(model: "gpt-4", apiKey: _apiKey);
        }

        public async Task<TopicDto> GetFlashcardsAsync(string topic)
        {
            var prompt0 = "[mandetory] Give me only the raw JSON array of English and Danish flashcards. Do not include any explanation, formatting, or code block markdown. I want only the JSON output in this format {{\r\n    \"English\": \"cat\",\r\n    \"Danish\": \"kat\"\r\n  },\r\n  ...}";
            var prompt1 = $"Generate 5 flashcards for the topic '{topic}', where each flashcard consists of an English word and its Danish translation.";

            // Send the prompt to OpenAI and get the response
            ChatCompletion completion = await _client.CompleteChatAsync([prompt0,prompt1]);

            // Access the first message's content and return it as a string
            var jsonText= completion.Content[0].Text;
            var topicDto = new TopicDto(topic);
            topicDto.Flashcards = JsonSerializer.Deserialize<FlashcardDto[]>(jsonText);
            return topicDto;
            
        }
    }
}


