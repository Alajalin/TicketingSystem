using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Core.DTOs.Auth;

public class AuthResponseDto
{
	public string Token { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string UserType { get; set; } = string.Empty;
	public int UserId { get; set; }
}