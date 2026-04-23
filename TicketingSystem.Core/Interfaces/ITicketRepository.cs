using System;
using System.Collections.Generic;
using System.Text;

using TicketingSystem.Core.Entities;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.Interfaces;

public interface ITicketRepository
{
	Task<Ticket?> GetByIdAsync(int id);
	Task<IEnumerable<Ticket>> GetAllAsync();
	Task<IEnumerable<Ticket>> GetByClientIdAsync(int clientId);
	Task<IEnumerable<Ticket>> GetByEmployeeIdAsync(int employeeId);
	Task<IEnumerable<Ticket>> GetFilteredAsync(TicketStatus? status, int? employeeId, int? clientId);
	Task<Ticket> AddAsync(Ticket ticket);
	Task UpdateAsync(Ticket ticket);
	Task<Dictionary<TicketStatus, int>> GetStatusCountsAsync();
	Task<IEnumerable<object>> GetTopEmployeesAsync(int top = 5);
}