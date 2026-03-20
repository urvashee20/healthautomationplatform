using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using SmartHealthCompanion.Data;
using SmartHealthCompanion.Entities;
using SmartHealthCompanion.Services;
using static SmartHealthCompanion.DTOs.CommonDto;

namespace SmartHealthCompanion.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly JwtService _jwtService;
        public AuthController(AppDbContext appDbContext, JwtService jwtService)
        {
            _appDbContext = appDbContext;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();

            return Ok("User Registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _appDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
                return Unauthorized("User not found");

            bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!isValid)
                return Unauthorized("Invalid password");

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                Token = token
            });
        }

        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecureEndpoint()
        {
            return Ok("You are authorized!");
        }
    }
}
