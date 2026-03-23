using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHealthCompanion.Data;
using SmartHealthCompanion.DTOs;
using SmartHealthCompanion.Entities;
using System.Security.Claims;

namespace SmartHealthCompanion.Controllers
{
    [Route("api/Profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile(UserProfileDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var existingProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId));

            if (existingProfile != null)
                return BadRequest("Profile already exists");

            var profile = new UserProfile
            {
                UserId = Guid.Parse(userId),
                FirstName = dto.FirstName,
               // MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                DOB = dto.DOB,
                Height = dto.Height,
                Weight = dto.Weight,
                JobType = dto.JobType,
                JobName = dto.JobName,
                JobStartTime = dto.JobStartTime,
                JobEndTime = dto.JobEndTime,
                DailyRoutine = dto.DailyRoutine,
                FoodPreference = dto.FoodPreference,
                HealthConditions = dto.HealthConditions,
                SleepHours = dto.SleepHours
            };

            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return Ok("Profile Created");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UserProfileDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId));

            if (profile == null)
                return NotFound("Profile not found");

            profile.FirstName = dto.FirstName;
            profile.LastName = dto.LastName;
            profile.Weight = dto.Weight;
            profile.Height = dto.Height;

            await _context.SaveChangesAsync();

            return Ok("Profile Updated");
        }
    }
}
