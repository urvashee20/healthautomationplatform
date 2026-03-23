using Microsoft.SemanticKernel;
using SmartHealthCompanion.DTOs;
using SmartHealthCompanion.Entities;
using System.Text.Json;

namespace SmartHealthCompanion.Services
{
    public class AIPlanService
    {
        private readonly GeminiService _geminiService;

        public AIPlanService(GeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        public async Task<AIPlan> GeneratePlanAsync(UserProfile profile, Goal? goal)
        {
            var prompt = BuildPrompt(profile, goal);

            var aiResponse = await _geminiService.GenerateAsync(prompt);

            var cleanedJson = ExtractJson(aiResponse);

            AIPlanDto? planDto = null;

            try
            {
                planDto = JsonSerializer.Deserialize<AIPlanDto>(cleanedJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
                planDto = new AIPlanDto
                {
                    DietPlan = aiResponse,
                    WorkoutPlan = "Not generated",
                    WaterPlan = "Not generated",
                    SleepPlan = "Not generated"
                };
            }

            return new AIPlan
            {
                UserProfileId = profile.Id,
                DietPlan = planDto?.DietPlan ?? "",
                WorkoutPlan = planDto?.WorkoutPlan ?? "",
                WaterPlan = planDto?.WaterPlan ?? "",
                SleepPlan = planDto?.SleepPlan ?? "",
                CreatedAt = DateTime.UtcNow
            };
        }

        private string BuildPrompt(UserProfile profile, Goal? goal)
        {
            return $@"
You are a professional AI health assistant.

User Profile:
- Age: {CalculateAge(profile.DOB)}
- Weight: {profile.Weight} kg
- Height: {profile.Height} cm
- Job Type: {profile.JobType}
- Daily Routine: {profile.DailyRoutine}
- Food Preference: {profile.FoodPreference}
- Sleep Hours: {profile.SleepHours}

Goal:
- Primary: {goal?.PrimaryGoal}
- Secondary: {goal?.SecondaryGoals}
- Health Conditions: {goal?.HealthConditions}
- Custom Goal: {goal?.CustomGoalText}

Instructions:
- Create a SIMPLE, PRACTICAL daily plan
- Focus on Indian lifestyle
- Keep workouts under 15 minutes
- Diet should be easy and quick meals

IMPORTANT:
Return ONLY valid JSON (no explanation)

Format:
{{
  ""dietPlan"": ""..."",
  ""workoutPlan"": ""..."",
  ""waterPlan"": ""..."",
  ""sleepPlan"": ""...""
}}
";
        }

        private string ExtractJson(string response)
        {
            int start = response.IndexOf("{");
            int end = response.LastIndexOf("}");

            if (start >= 0 && end > start)
            {
                return response.Substring(start, end - start + 1);
            }

            return response; // fallback
        }

        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            int age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;
            return age;
        }
    }
}
