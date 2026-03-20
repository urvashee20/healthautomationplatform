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
        public async Task<IActionResult> SetGoal(GoalDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId));

            if (profile == null)
                return BadRequest("Create profile first");

            var goal = new Goal
            {
                UserProfileId = profile.Id,
                GoalType = dto.GoalType,
                TargetWeight = dto.TargetWeight,
                StartDate = DateTime.UtcNow
            };

            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();

            return Ok("Goal Set Successfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetGoal()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId));

            var goal = await _context.Goals
                .Where(x => x.UserProfileId == profile.Id)
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefaultAsync();

            return Ok(goal);
        }
    }
}
