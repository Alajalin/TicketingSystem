using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.DTOs.Dashboard;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.Interfaces;

namespace TicketingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class DashboardController : ControllerBase
{
	private readonly ITicketRepository _ticketRepo;
	private readonly IUserRepository _userRepo;

	public DashboardController(ITicketRepository ticketRepo, IUserRepository userRepo)
	{
		_ticketRepo = ticketRepo;
		_userRepo = userRepo;
	}

	[HttpGet]
	public async Task<IActionResult> GetDashboard()
	{
		var statusCounts = await _ticketRepo.GetStatusCountsAsync();
		var topEmployees = await _ticketRepo.GetTopEmployeesAsync();
		var clients = await _userRepo.GetAllByTypeAsync(UserType.ExternalClient);
		var employees = await _userRepo.GetAllByTypeAsync(UserType.SupportEmployee);

		var dto = new DashboardDto
		{
			TotalTickets = statusCounts.Values.Sum(),
			NewTickets = statusCounts.GetValueOrDefault(TicketStatus.New),
			AssignedTickets = statusCounts.GetValueOrDefault(TicketStatus.Assigned),
			InProgressTickets = statusCounts.GetValueOrDefault(TicketStatus.InProgress),
			ClosedTickets = statusCounts.GetValueOrDefault(TicketStatus.Closed),
			TotalClients = clients.Count(),
			TotalEmployees = employees.Count(),
			TopEmployees = topEmployees.Select(e =>
			{
				dynamic d = e;
				return new EmployeeProductivityDto
				{
					EmployeeName = d.EmployeeName,
					ClosedTickets = d.ClosedTickets
				};
			}).ToList(),
			StatusChart = statusCounts.Select(kv => new StatusChartDto
			{
				Status = kv.Key.ToString(),
				Count = kv.Value
			}).ToList()
		};

		return Ok(dto);
	}
}