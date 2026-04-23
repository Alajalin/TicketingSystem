using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Core.DTOs.Auth;

public class LoginDto
{
	public string EmailOrUsername { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}