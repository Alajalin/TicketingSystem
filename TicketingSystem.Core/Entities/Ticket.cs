using System;
using System.Collections.Generic;
using System.Text;

using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.Entities;

public class Ticket
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string ProblemDescription { get; set; } = string.Empty;
	public TicketStatus Status { get; set; } = TicketStatus.New;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime? UpdatedAt { get; set; }
	public DateTime? ClosedAt { get; set; }

	public int ClientId { get; set; }
	public User Client { get; set; } = null!;

	public int ProductId { get; set; }
	public Product Product { get; set; } = null!;

	public int? AssignedEmployeeId { get; set; }
	public User? AssignedEmployee { get; set; }

	public ICollection<TicketAttachment> Attachments { get; set; } = new List<TicketAttachment>();
	public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
}