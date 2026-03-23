using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthCompanion.Data;
using SmartHealthCompanion.Entities;
using System.Security.Claims;
using static SmartHealthCompanion.DTOs.CommonDto;

namespace SmartHealthCompanion.Controllers
{
    [Route("api/Goal")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GoalController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> SetGoal(CreateGoalDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId));

            if (profile == null)
                return BadRequest("Create profile first");

            var goal = new Goal
            {
                UserProfileId = profile.Id,
                PrimaryGoal = dto.PrimaryGoal,
                SecondaryGoals = dto.SecondaryGoals != null
                    ? System.Text.Json.JsonSerializer.Serialize(dto.SecondaryGoals)
                    : null,
                HealthConditions = dto.HealthConditions != null
                    ? System.Text.Json.JsonSerializer.Serialize(dto.HealthConditions)
                    : null,
                IsCustomGoal = dto.IsCustomGoal,
                CustomGoalText = dto.CustomGoalText,
                TargetWeight = dto.TargetWeight,
                DurationInDays = dto.DurationInDays,
                HasGymAccess = dto.HasGymAccess,
                SleepHours = dto.SleepHours,
                CreatedAt = DateTime.UtcNow
            };

            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Goal set successfully",
                goalId = goal.Id
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetGoal()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId));

            if (profile == null)
                return NotFound("Profile not found");

            var goal = await _context.Goals
                .Where(x => x.UserProfileId == profile.Id)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            if (goal == null)
                return Ok("No goal set");

            var result = new
            {
                goal.Id,
                goal.PrimaryGoal,
                SecondaryGoals = !string.IsNullOrEmpty(goal.SecondaryGoals)
                    ? System.Text.Json.JsonSerializer.Deserialize<List<string>>(goal.SecondaryGoals)
                    : null,
                HealthConditions = !string.IsNullOrEmpty(goal.HealthConditions)
                    ? System.Text.Json.JsonSerializer.Deserialize<List<string>>(goal.HealthConditions)
                    : null,
                goal.IsCustomGoal,
                goal.CustomGoalText,
                goal.TargetWeight,
                goal.DurationInDays,
                goal.HasGymAccess,
                goal.SleepHours,
                goal.CreatedAt
            };

            return Ok(result);
        }
    }
}
