using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Entities;
using TicketingSystem.Core.Interfaces;
using TicketingSystem.DataAccess.Context;

namespace TicketingSystem.DataAccess.Repositories;

public class CommentRepository : ICommentRepository
{
	private readonly AppDbContext _context;

	public CommentRepository(AppDbContext context) => _context = context;

	public async Task<IEnumerable<TicketComment>> GetByTicketIdAsync(int ticketId) =>
		await _context.TicketComments
			.Include(c => c.User)
			.Where(c => c.TicketId == ticketId)
			.OrderBy(c => c.CreatedAt)
			.ToListAsync();

	public async Task<TicketComment> AddAsync(TicketComment comment)
	{
		_context.TicketComments.Add(comment);
		await _context.SaveChangesAsync();
		return comment;
	}
}
