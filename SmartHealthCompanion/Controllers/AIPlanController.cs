using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthCompanion.Data;
using SmartHealthCompanion.DTOs;
using SmartHealthCompanion.Entities;
using SmartHealthCompanion.Services;
using System.Security.Claims;
using static SmartHealthCompanion.DTOs.CommonDto;

namespace SmartHealthCompanion.Controllers
{
    [Route("api/AIPlan")]
    [ApiController]
    public class AIPlanController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AIPlanService _aiService;
        private readonly IMapper _mapper;
        public AIPlanController(AppDbContext context, AIPlanService aiService,IMapper mapper)
        {
            _context = context;
            _aiService = aiService;
            _mapper = mapper;
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

            return Ok(new AIPlanResponseDto
            {
                Id = plan.Id,
                GoalId = plan.GoalId,
                DietPlan = plan.DietPlan,
                WorkoutPlan = plan.WorkoutPlan,
                WaterPlan = plan.WaterPlan,
                SleepPlan = plan.SleepPlan,
                CreatedAt = plan.CreatedAt
            });
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

        [HttpGet]
        public async Task<IActionResult> GetPlanByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var userGuid = Guid.Parse(userId);

            var profile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userGuid);

            if (profile == null)
            {
                return Ok(new
                {
                    profile = (UserProfileDto)null,
                    goal = (CreateGoalDto)null,
                    plan = (AIPlanDto)null
                });
            }

            var goal = await _context.Goals
                .Where(x => x.UserProfileId == profile.Id)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            var plan = await _context.AIPlan
                .Where(x => goal != null && x.GoalId == goal.Id)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            var profileDto = _mapper.Map<UserProfileDto>(profile);
            var goalDto = goal != null ? _mapper.Map<CreateGoalDto>(goal) : null;
            var planDto = plan != null ? _mapper.Map<AIPlanDto>(plan) : null;

            return Ok(new
            {
                profile = profileDto,
                goal = goalDto,
                plan = planDto
            });
        }
    }
}
