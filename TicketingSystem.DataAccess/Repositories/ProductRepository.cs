using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Entities;
using TicketingSystem.Core.Interfaces;
using TicketingSystem.DataAccess.Context;

namespace TicketingSystem.DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
	private readonly AppDbContext _context;

	public ProductRepository(AppDbContext context) => _context = context;

	public async Task<IEnumerable<Product>> GetAllActiveAsync() =>
		await _context.Products.Where(p => p.IsActive).ToListAsync();

	public async Task<Product?> GetByIdAsync(int id) =>
		await _context.Products.FindAsync(id);

	public async Task<Product> AddAsync(Product product)
	{
		_context.Products.Add(product);
		await _context.SaveChangesAsync();
		return product;
	}
}
