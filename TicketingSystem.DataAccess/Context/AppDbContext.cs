using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Entities;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.DataAccess.Context;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<User> Users => Set<User>();
	public DbSet<Product> Products => Set<Product>();
	public DbSet<Ticket> Tickets => Set<Ticket>();
	public DbSet<TicketComment> TicketComments => Set<TicketComment>();
	public DbSet<TicketAttachment> TicketAttachments => Set<TicketAttachment>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>(e =>
		{
			e.HasIndex(u => u.Email).IsUnique();
			e.Property(u => u.UserType).HasConversion<string>();
		});

		modelBuilder.Entity<Ticket>(e =>
		{
			e.Property(t => t.Status).HasConversion<string>();

			e.HasOne(t => t.Client)
				.WithMany(u => u.SubmittedTickets)
				.HasForeignKey(t => t.ClientId)
				.OnDelete(DeleteBehavior.Restrict);

			e.HasOne(t => t.AssignedEmployee)
				.WithMany(u => u.AssignedTickets)
				.HasForeignKey(t => t.AssignedEmployeeId)
				.OnDelete(DeleteBehavior.Restrict);
		});

		modelBuilder.Entity<TicketComment>(e =>
		{
			e.HasOne(c => c.Ticket)
				.WithMany(t => t.Comments)
				.HasForeignKey(c => c.TicketId)
				.OnDelete(DeleteBehavior.Cascade);

			e.HasOne(c => c.User)
				.WithMany(u => u.Comments)
				.HasForeignKey(c => c.UserId)
				.OnDelete(DeleteBehavior.Restrict);
		});

		SeedData(modelBuilder);
	}

	private static void SeedData(ModelBuilder modelBuilder)
	{
		const string managerPasswordHash =
			"$2a$11$nf/wN9a1oPJf0B6fPaS/VeFo0dGZN2ehVQ8.4xrLdhfegNk8oXbOK";

		modelBuilder.Entity<User>().HasData(new User
		{
			Id = 1,
			FullName = "Support Manager",
			Email = "manager@ticketing.com",
			MobileNumber = "0500000000",
			PasswordHash = managerPasswordHash,
			DateOfBirth = new DateTime(1985, 1, 1),
			UserType = UserType.Manager,
			IsActive = true,
			CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
		});

		modelBuilder.Entity<Product>().HasData(
			new Product { Id = 1, Name = "Product A", Description = "Main product", IsActive = true },
			new Product { Id = 2, Name = "Product B", Description = "Second product", IsActive = true },
			new Product { Id = 3, Name = "Product C", Description = "Third product", IsActive = true }
		);
	}
}
