using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Core.DTOs.Ticket;

public class CommentDto
{
	public int Id { get; set; }
	public string Content { get; set; } = string.Empty;
	public string AuthorName { get; set; } = string.Empty;
	public string AuthorType { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
}