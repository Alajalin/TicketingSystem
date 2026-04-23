using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketingSystem.Core.DTOs.User;
using TicketingSystem.Core.Entities;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.Interfaces;

namespace TicketingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
	private readonly IUserRepository _userRepo;
	private readonly ILogger<UsersController> _logger;

	public UsersController(IUserRepository userRepo, ILogger<UsersController> logger)
	{
		_userRepo = userRepo;
		_logger = logger;
	}

	[HttpGet("employees")]
	[Authorize(Roles = "Manager")]
	public async Task<IActionResult> GetEmployees()
	{
		var employees = await _userRepo.GetAllByTypeAsync(UserType.SupportEmployee);
		return Ok(employees.Select(MapToDto));
	}

	[HttpGet("clients")]
	[Authorize(Roles = "Manager")]
	public async Task<IActionResult> GetClients()
	{
		var clients = await _userRepo.GetAllByTypeAsync(UserType.ExternalClient);
		return Ok(clients.Select(MapToDto));
	}

	[HttpGet("{id}")]
	[Authorize(Roles = "Manager")]
	public async Task<IActionResult> GetById(int id)
	{
		var user = await _userRepo.GetByIdAsync(id);
		if (user == null) return NotFound();
		return Ok(MapToDto(user));
	}

	[HttpPost("employees")]
	[Authorize(Roles = "Manager")]
	public async Task<IActionResult> AddEmployee([FromBody] RegisterEmployeeDto dto)
	{
		if (await _userRepo.EmailExistsAsync(dto.Email))
			return BadRequest(new { message = "Email already exists" });

		var employee = new User
		{
			FullName = dto.FullName,
			Email = dto.Email.ToLower(),
			MobileNumber = dto.MobileNumber,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
			DateOfBirth = dto.DateOfBirth,
			Address = dto.Address,
			UserType = UserType.SupportEmployee,
			IsActive = true
		};

		await _userRepo.AddAsync(employee);
		_logger.LogInformation("Manager added new employee: {Email}", employee.Email);
		return Ok(MapToDto(employee));
	}

	[HttpPut("{id}/toggle-active")]
	[Authorize(Roles = "Manager")]
	public async Task<IActionResult> ToggleActive(int id)
	{
		var user = await _userRepo.GetByIdAsync(id);
		if (user == null) return NotFound();

		user.IsActive = !user.IsActive;
		await _userRepo.UpdateAsync(user);
		_logger.LogInformation("User {Id} active status toggled to {Status}",
			id, user.IsActive);
		return Ok(new { isActive = user.IsActive });
	}

	[HttpPut("{id}")]
	[Authorize(Roles = "Manager")]
	public async Task<IActionResult> Update(int id, [FromBody] RegisterEmployeeDto dto)
	{
		var user = await _userRepo.GetByIdAsync(id);
		if (user == null) return NotFound();

		user.FullName = dto.FullName;
		user.MobileNumber = dto.MobileNumber;
		user.DateOfBirth = dto.DateOfBirth;
		user.Address = dto.Address;

		await _userRepo.UpdateAsync(user);
		return Ok(MapToDto(user));
	}

	[HttpGet("profile")]
	public async Task<IActionResult> GetProfile()
	{
		var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
		var user = await _userRepo.GetByIdAsync(userId);
		if (user == null) return NotFound();
		return Ok(MapToDto(user));
	}

	private static UserDto MapToDto(User u) => new()
	{
		Id = u.Id,
		FullName = u.FullName,
		Email = u.Email,
		MobileNumber = u.MobileNumber,
		ImagePath = u.ImagePath,
		DateOfBirth = u.DateOfBirth,
		Address = u.Address,
		UserType = u.UserType.ToString(),
		IsActive = u.IsActive,
		CreatedAt = u.CreatedAt
	};
}