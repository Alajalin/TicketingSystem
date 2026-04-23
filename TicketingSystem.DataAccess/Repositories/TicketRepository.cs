using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Entities;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.Interfaces;
using TicketingSystem.DataAccess.Context;

namespace TicketingSystem.DataAccess.Repositories;

public class TicketRepository : ITicketRepository
{
	private readonly AppDbContext _context;

	public TicketRepository(AppDbContext context) => _context = context;

	public async Task<Ticket?> GetByIdAsync(int id) =>
		await _context.Tickets
			.Include(t => t.Client)
			.Include(t => t.Product)
			.Include(t => t.AssignedEmployee)
			.Include(t => t.Comments).ThenInclude(c => c.User)
			.Include(t => t.Attachments)
			.FirstOrDefaultAsync(t => t.Id == id);

	public async Task<IEnumerable<Ticket>> GetAllAsync() =>
		await _context.Tickets
			.Include(t => t.Client)
			.Include(t => t.Product)
			.Include(t => t.AssignedEmployee)
			.OrderByDescending(t => t.CreatedAt)
			.ToListAsync();

	public async Task<IEnumerable<Ticket>> GetByClientIdAsync(int clientId) =>
		await _context.Tickets
			.Include(t => t.Product)
			.Include(t => t.AssignedEmployee)
			.Where(t => t.ClientId == clientId)
			.OrderByDescending(t => t.CreatedAt)
			.ToListAsync();

	public async Task<IEnumerable<Ticket>> GetByEmployeeIdAsync(int employeeId) =>
		await _context.Tickets
			.Include(t => t.Client)
			.Include(t => t.Product)
			.Where(t => t.AssignedEmployeeId == employeeId)
			.OrderByDescending(t => t.CreatedAt)
			.ToListAsync();

	public async Task<IEnumerable<Ticket>> GetFilteredAsync(
		TicketStatus? status, int? employeeId, int? clientId)
	{
		var query = _context.Tickets
			.Include(t => t.Client)
			.Include(t => t.Product)
			.Include(t => t.AssignedEmployee)
			.AsQueryable();

		if (status.HasValue) query = query.Where(t => t.Status == status.Value);
		if (employeeId.HasValue) query = query.Where(t => t.AssignedEmployeeId == employeeId.Value);
		if (clientId.HasValue) query = query.Where(t => t.ClientId == clientId.Value);

		return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
	}

	public async Task<Ticket> AddAsync(Ticket ticket)
	{
		_context.Tickets.Add(ticket);
		await _context.SaveChangesAsync();
		return ticket;
	}

	public async Task UpdateAsync(Ticket ticket)
	{
		ticket.UpdatedAt = DateTime.UtcNow;
		_context.Tickets.Update(ticket);
		await _context.SaveChangesAsync();
	}

	public async Task<Dictionary<TicketStatus, int>> GetStatusCountsAsync()
	{
		return await _context.Tickets
			.GroupBy(t => t.Status)
			.ToDictionaryAsync(g => g.Key, g => g.Count());
	}

	public async Task<IEnumerable<object>> GetTopEmployeesAsync(int top = 5)
	{
		return await _context.Tickets
			.Where(t => t.Status == TicketStatus.Closed && t.AssignedEmployeeId != null)
			.GroupBy(t => new { t.AssignedEmployeeId, t.AssignedEmployee!.FullName })
			.Select(g => new { EmployeeName = g.Key.FullName, ClosedTickets = g.Count() })
			.OrderByDescending(x => x.ClosedTickets)
			.Take(top)
			.ToListAsync<object>();
	}
}