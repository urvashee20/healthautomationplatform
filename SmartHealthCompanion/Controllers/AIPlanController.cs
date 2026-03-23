using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthCompanion.Data;
using SmartHealthCompanion.Services;
using System.Security.Claims;

namespace SmartHealthCompanion.Controllers
{
    [Route("api/AIPlan")]
    [ApiController]
    public class AIPlanController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AIPlanService _aiService;

        public AIPlanController(AppDbContext context, AIPlanService aiService)
        {
            _context = context;
            _aiService = aiService;
        }
        [HttpPost("generate-plan")]
        public async Task<IActionResult> GeneratePlan()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId));

            if (profile == null)
                return BadRequest("Profile not found");

            var goal = await _context.Goals
                .Where(x => x.UserProfileId == profile.Id)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            var plan = await _aiService.GeneratePlanAsync(profile, goal);

            _context.AIPlan.Add(plan);
            await _context.SaveChangesAsync();

            return Ok(plan);
        }

        [HttpGet("latest-plan")]
        public async Task<IActionResult> GetLatestPlan()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId));

            var plan = await _context.AIPlan
                .Where(x => x.UserProfileId == profile.Id)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            return Ok(plan);
        }
    }
}
