using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Entities;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.Interfaces;
using TicketingSystem.DataAccess.Context;

namespace TicketingSystem.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
	private readonly AppDbContext _context;

	public UserRepository(AppDbContext context) => _context = context;

	public async Task<User?> GetByIdAsync(int id) =>
		await _context.Users.FindAsync(id);

	public async Task<User?> GetByEmailAsync(string email) =>
		await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower());

	public async Task<IEnumerable<User>> GetAllByTypeAsync(UserType userType) =>
		await _context.Users.Where(u => u.UserType == userType).ToListAsync();

	public async Task<IEnumerable<User>> GetAllAsync() =>
		await _context.Users.ToListAsync();

	public async Task<User> AddAsync(User user)
	{
		_context.Users.Add(user);
		await _context.SaveChangesAsync();
		return user;
	}

	public async Task UpdateAsync(User user)
	{
		_context.Users.Update(user);
		await _context.SaveChangesAsync();
	}

	public async Task<bool> EmailExistsAsync(string email) =>
		await _context.Users.AnyAsync(u => u.Email == email.ToLower());
}