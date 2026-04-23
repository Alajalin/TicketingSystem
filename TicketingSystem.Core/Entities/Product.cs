using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TicketingSystem.Core.Entities;

public class Product
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public bool IsActive { get; set; } = true;

	public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}