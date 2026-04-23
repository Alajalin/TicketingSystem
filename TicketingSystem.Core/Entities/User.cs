using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.Entities;

public class User
{
	public int Id { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string MobileNumber { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
	public string? ImagePath { get; set; }
	public DateTime DateOfBirth { get; set; }
	public string? Address { get; set; }
	public UserType UserType { get; set; }
	public bool IsActive { get; set; } = true;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public ICollection<Ticket> SubmittedTickets { get; set; } = new List<Ticket>();
	public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
	public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
}