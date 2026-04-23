using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Core.DTOs.Ticket;

public class TicketDto
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string ProblemDescription { get; set; } = string.Empty;
	public string Status { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public DateTime? ClosedAt { get; set; }
	public string ClientName { get; set; } = string.Empty;
	public int ClientId { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public int ProductId { get; set; }
	public string? AssignedEmployeeName { get; set; }
	public int? AssignedEmployeeId { get; set; }
	public List<CommentDto> Comments { get; set; } = new();
	public List<string> Attachments { get; set; } = new();
}