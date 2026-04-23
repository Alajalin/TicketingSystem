using System;
using System.Collections.Generic;
using System.Text;

using TicketingSystem.Core.Entities;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.Interfaces;

public interface IUserRepository
{
	Task<User?> GetByIdAsync(int id);
	Task<User?> GetByEmailAsync(string email);
	Task<IEnumerable<User>> GetAllByTypeAsync(UserType userType);
	Task<IEnumerable<User>> GetAllAsync();
	Task<User> AddAsync(User user);
	Task UpdateAsync(User user);
	Task<bool> EmailExistsAsync(string email);
}