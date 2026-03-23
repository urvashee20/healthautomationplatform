using System.Net.Http.Json;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;

namespace SmartHealthCompanion.Services
{
    public class GeminiService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public GeminiService(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<string> GenerateAsync(string prompt)
        {
            var apiKey = _config["Gemini:ApiKey"];
            var configuredModel = _config["Gemini:Model"];

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("Gemini:ApiKey is not configured.");

            if (string.IsNullOrWhiteSpace(configuredModel))
                throw new InvalidOperationException("Gemini:Model is not configured. Call the ListModels endpoint to choose a valid model.");

            // Build candidate model ids to try (both "models/..." and the bare id)
            var candidates = new List<string>();
            var trimmed = configuredModel.StartsWith("models/") ? configuredModel.Substring("models/".Length) : configuredModel;
            candidates.Add(trimmed);
            if (!configuredModel.StartsWith("models/"))
                candidates.Add("models/" + trimmed);
            else
                candidates.Add(configuredModel);

            // Also ensure candidates include names found from ListModels (helps if config is slightly different)
            try
            {
                var available = await ListModelsAsync(apiKey);
                foreach (var a in available)
                {
                    var id = a.StartsWith("models/") ? a.Substring("models/".Length) : a;
                    if (!candidates.Contains(id))
                        candidates.Add(id);
                    if (!candidates.Contains(a))
                        candidates.Add(a);
                }
            }
            catch
            {
                // ignore list models failure — we'll still try configured candidates
            }

            var lastStatus = new List<string>();
            foreach (var candidate in candidates.Distinct())
            {
                // Use the candidate in the path. If candidate starts with "models/" remove the duplicate segment
                var modelPath = candidate.StartsWith("models/") ? candidate.Substring("models/".Length) : candidate;
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelPath}:generateContent?key={apiKey}";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                HttpResponseMessage response;
                try
                {
                    response = await _httpClient.PostAsJsonAsync(url, requestBody);
                }
                catch (Exception ex)
                {
                    lastStatus.Add($"Attempt {candidate} -> request error: {ex.Message}");
                    continue;
                }

                var body = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        using var doc = JsonDocument.Parse(body);
                        var text = doc.RootElement
                            .GetProperty("candidates")[0]
                            .GetProperty("content")
                            .GetProperty("parts")[0]
                            .GetProperty("text")
                            .GetString();
                        return text ?? "";
                    }
                    catch (Exception ex)
                    {
                        lastStatus.Add($"Attempt {candidate} -> parse error: {ex.Message}");
                        continue;
                    }
                }

                // Collect reasons for failure for diagnostics
                lastStatus.Add($"Attempt {candidate} -> {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

                // If 404 specifically, try next candidate
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    continue;

                // For other failure codes, continue trying candidates but keep diagnostics
            }

            // If we get here, none of the candidates succeeded. Provide details and guidance.
            var detail = string.Join(" | ", lastStatus);
            throw new Exception("Gemini Error: none of the model endpoints succeeded. Tried candidates: "
                                + string.Join(", ", candidates.Distinct()) + ". Responses: " + detail
                                + ". Ensure the configured model is supported for the :generateContent method, the Generative Language API is enabled in the Google Cloud project, and your API key has access. Consider using OAuth2 service account credentials if required.");
        }

        // Helper to list models to help debug configuration problems
        public async Task<List<string>> ListModelsAsync(string apiKey)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models?key={apiKey}";
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to list Gemini models: {json}");
            }

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var models = new List<string>();
                if (root.TryGetProperty("models", out var modelsElement) && modelsElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var m in modelsElement.EnumerateArray())
                    {
                        if (m.TryGetProperty("name", out var nameProp))
                        {
                            models.Add(nameProp.GetString() ?? string.Empty);
                        }
                    }
                }

                return models;
            }
            catch (JsonException)
            {
                throw new Exception("Unable to parse ListModels response: " + json);
            }
        }
    }
}
