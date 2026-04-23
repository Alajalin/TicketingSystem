using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Core.DTOs.User;

public class UserDto
{
	public int Id { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string MobileNumber { get; set; } = string.Empty;
	public string? ImagePath { get; set; }
	public DateTime DateOfBirth { get; set; }
	public string? Address { get; set; }
	public string UserType { get; set; } = string.Empty;
	public bool IsActive { get; set; }
	public DateTime CreatedAt { get; set; }
	public int TicketCount { get; set; }
}
