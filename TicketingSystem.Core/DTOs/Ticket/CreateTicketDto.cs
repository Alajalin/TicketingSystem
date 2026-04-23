using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Core.DTOs.Ticket;

public class CreateTicketDto
{
	public string Title { get; set; } = string.Empty;
	public string ProblemDescription { get; set; } = string.Empty;
	public int ProductId { get; set; }
}