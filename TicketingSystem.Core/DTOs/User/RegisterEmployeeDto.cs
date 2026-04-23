using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Core.DTOs.User;

public class RegisterEmployeeDto
{
	public string FullName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string MobileNumber { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public DateTime DateOfBirth { get; set; }
	public string? Address { get; set; }
}