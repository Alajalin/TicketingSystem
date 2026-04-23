using System;
using System.Collections.Generic;
using System.Text;

using TicketingSystem.Core.Entities;

namespace TicketingSystem.Core.Interfaces;

public interface IProductRepository
{
	Task<IEnumerable<Product>> GetAllActiveAsync();
	Task<Product?> GetByIdAsync(int id);
	Task<Product> AddAsync(Product product);
}