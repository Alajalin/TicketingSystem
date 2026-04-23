using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Core.Entities;

public class TicketAttachment
{
	public int Id { get; set; }
	public string FileName { get; set; } = string.Empty;
	public string FilePath { get; set; } = string.Empty;
	public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

	public int TicketId { get; set; }
	public Ticket Ticket { get; set; } = null!;
}