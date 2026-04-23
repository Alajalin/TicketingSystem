using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketingSystem.Core.DTOs.Auth;
using TicketingSystem.Core.DTOs.User;
using TicketingSystem.Core.Entities;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.Interfaces;

namespace TicketingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IUserRepository _userRepo;
	private readonly IConfiguration _config;
	private readonly ILogger<AuthController> _logger;

	public AuthController(IUserRepository userRepo, IConfiguration config,
		ILogger<AuthController> logger)
	{
		_userRepo = userRepo;
		_config = config;
		_logger = logger;
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto dto)
	{
		var user = await _userRepo.GetByEmailAsync(dto.EmailOrUsername);
		if (user == null || !user.IsActive ||
			!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
		{
			_logger.LogWarning("Failed login attempt for {Email}", dto.EmailOrUsername);
			return Unauthorized(new { message = "Invalid credentials or account is inactive" });
		}

		var token = GenerateToken(user);
		_logger.LogInformation("User {Email} logged in successfully", user.Email);

		return Ok(new AuthResponseDto
		{
			Token = token,
			FullName = user.FullName,
			Email = user.Email,
			UserType = user.UserType.ToString(),
			UserId = user.Id
		});
	}

	[HttpPost("register-client")]
	public async Task<IActionResult> RegisterClient([FromBody] RegisterClientDto dto)
	{
		if (await _userRepo.EmailExistsAsync(dto.Email))
			return BadRequest(new { message = "Email already registered" });

		var user = new User
		{
			FullName = dto.FullName,
			Email = dto.Email.ToLower(),
			MobileNumber = dto.MobileNumber,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
			DateOfBirth = dto.DateOfBirth,
			Address = dto.Address,
			UserType = UserType.ExternalClient,
			IsActive = true
		};

		await _userRepo.AddAsync(user);
		_logger.LogInformation("New client registered: {Email}", user.Email);

		return Ok(new { message = "Registration successful. You can now login." });
	}

	private string GenerateToken(User user)
	{
		var key = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Name, user.FullName),
			new Claim(ClaimTypes.Role, user.UserType.ToString())
		};

		var token = new JwtSecurityToken(
			issuer: _config["Jwt:Issuer"],
			audience: _config["Jwt:Audience"],
			claims: claims,
			expires: DateTime.UtcNow.AddHours(8),
			signingCredentials: creds
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}