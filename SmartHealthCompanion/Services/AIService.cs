using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartHealthCompanion.Entities;
using SmartHealthCompanion.Interfaces;

namespace SmartHealthCompanion.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<AIService> _logger;

        public AIService(HttpClient httpClient, IConfiguration config, ILogger<AIService> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
        }

        public async Task<string> GetChatResponseAsync(string userMessage, IEnumerable<ChatMessage> previousMessages)
        {
            try
            {
                var apiKey = _config["Gemini:ApiKey"];
                var configuredModel = _config["Gemini:Model"] ?? "gemini-1.5-flash";

                // Mock response if API key not available or empty
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    _logger.LogWarning("Gemini API key is missing. Returning mocked AI response.");
                    await Task.Delay(1000); // Simulate network latency
                    return $"[Mocked] I see you said: '{userMessage}'. This is a simulated response since no API key was found.";
                }

                var modelPath = configuredModel.StartsWith("models/") ? configuredModel.Substring("models/".Length) : configuredModel;
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelPath}:generateContent?key={apiKey}";

                // Build context from previous messages + current user message
                // In Gemini format, each part needs to be text.
                var contents = new List<object>();

                foreach (var msg in previousMessages)
                {
                    contents.Add(new
                    {
                        role = "user",
                        parts = new[] { new { text = msg.UserMessage } }
                    });

                    if (!string.IsNullOrEmpty(msg.AIResponse))
                    {
                        contents.Add(new
                        {
                            role = "model",
                            parts = new[] { new { text = msg.AIResponse } }
                        });
                    }
                }

                contents.Add(new
                {
                    role = "user",
                    parts = new[] { new { text = userMessage } }
                });

                var requestBody = new
                {
                    contents = contents.ToArray()
                };

                var response = await _httpClient.PostAsJsonAsync(url, requestBody);
                var body = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(body);
                    var text = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();
                    
                    return text ?? "No response generated.";
                }

                _logger.LogError("AI Service error: {StatusCode} - {Body}", response.StatusCode, body);
                throw new Exception($"Failed to get AI response. Status: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in AIService while generating response.");
                throw;
            }
        }
    }
}
