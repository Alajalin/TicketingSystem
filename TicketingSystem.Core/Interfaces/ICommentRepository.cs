using System;
using System.Collections.Generic;
using System.Text;

using TicketingSystem.Core.Entities;

namespace TicketingSystem.Core.Interfaces;

public interface ICommentRepository
{
	Task<IEnumerable<TicketComment>> GetByTicketIdAsync(int ticketId);
	Task<TicketComment> AddAsync(TicketComment comment);
}