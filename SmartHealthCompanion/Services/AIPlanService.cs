using Microsoft.SemanticKernel;
using SmartHealthCompanion.Entities;
using System.Text.Json;
using static SmartHealthCompanion.DTOs.CommonDto;

namespace SmartHealthCompanion.Services
{
    public class AIPlanService
    {
        private readonly Kernel _kernel;
        public AIPlanService(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<AIPlan> GeneratePlanAsync(UserProfile profile, Goal? goal)
        {
            var prompt = BuildPrompt(profile, goal);

            var result = await _kernel.InvokePromptAsync(prompt);

            var aiResponse = result.GetValue<string>();

            // 🧠 Parse structured JSON
            var planDto = JsonSerializer.Deserialize<AIPlanDto>(aiResponse);

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

        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            int age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;
            return age;
        }
    }
}
