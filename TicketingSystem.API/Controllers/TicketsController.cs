using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketingSystem.Core.DTOs.Ticket;
using TicketingSystem.Core.Entities;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.Interfaces;

namespace TicketingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
	private readonly ITicketRepository _ticketRepo;
	private readonly ICommentRepository _commentRepo;
	private readonly ILogger<TicketsController> _logger;
	private readonly IWebHostEnvironment _env;

	public TicketsController(ITicketRepository ticketRepo, ICommentRepository commentRepo,
		ILogger<TicketsController> logger, IWebHostEnvironment env)
	{
		_ticketRepo = ticketRepo;
		_commentRepo = commentRepo;
		_logger = logger;
		_env = env;
	}

	[HttpGet]
	[Authorize(Roles = "Manager")]
	public async Task<IActionResult> GetAll([FromQuery] string? status,
		[FromQuery] int? employeeId, [FromQuery] int? clientId)
	{
		TicketStatus? ticketStatus = null;
		if (!string.IsNullOrEmpty(status) &&
			Enum.TryParse<TicketStatus>(status, out var parsed))
			ticketStatus = parsed;

		var tickets = await _ticketRepo.GetFilteredAsync(ticketStatus, employeeId, clientId);
		return Ok(tickets.Select(MapToDto));
	}

	[HttpGet("my-tickets")]
	[Authorize(Roles = "ExternalClient")]
	public async Task<IActionResult> GetMyTickets()
	{
		var clientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
		var tickets = await _ticketRepo.GetByClientIdAsync(clientId);
		return Ok(tickets.Select(MapToDto));
	}

	[HttpGet("assigned")]
	[Authorize(Roles = "SupportEmployee")]
	public async Task<IActionResult> GetAssignedTickets()
	{
		var employeeId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
		var tickets = await _ticketRepo.GetByEmployeeIdAsync(employeeId);
		return Ok(tickets.Select(MapToDto));
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		var ticket = await _ticketRepo.GetByIdAsync(id);
		if (ticket == null) return NotFound();

		var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
		var role = User.FindFirstValue(ClaimTypes.Role);

		if (role == "ExternalClient" && ticket.ClientId != userId)
			return Forbid();
		if (role == "SupportEmployee" && ticket.AssignedEmployeeId != userId)
			return Forbid();

		return Ok(MapToDto(ticket));
	}

	[HttpPost]
	[Authorize(Roles = "ExternalClient")]
	public async Task<IActionResult> Create([FromBody] CreateTicketDto dto)
	{
		var clientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

		var ticket = new Ticket
		{
			Title = dto.Title,
			ProblemDescription = dto.ProblemDescription,
			ProductId = dto.ProductId,
			ClientId = clientId,
			Status = TicketStatus.New
		};

		await _ticketRepo.AddAsync(ticket);
		_logger.LogInformation("New ticket created by client {ClientId}", clientId);
		return Ok(new { ticket.Id, message = "Ticket submitted successfully" });
	}

	[HttpPost("{id}/assign")]
	[Authorize(Roles = "Manager")]
	public async Task<IActionResult> Assign(int id, [FromBody] AssignTicketDto dto)
	{
		var ticket = await _ticketRepo.GetByIdAsync(id);
		if (ticket == null) return NotFound();

		if (ticket.AssignedEmployeeId.HasValue)
			return BadRequest(new { message = "Ticket is already assigned" });

		ticket.AssignedEmployeeId = dto.EmployeeId;
		ticket.Status = TicketStatus.Assigned;
		await _ticketRepo.UpdateAsync(ticket);

		_logger.LogInformation("Ticket {TicketId} assigned to employee {EmployeeId}",
			id, dto.EmployeeId);
		return Ok(new { message = "Ticket assigned successfully" });
	}

	[HttpPost("{id}/close")]
	[Authorize(Roles = "SupportEmployee")]
	public async Task<IActionResult> Close(int id)
	{
		var ticket = await _ticketRepo.GetByIdAsync(id);
		if (ticket == null) return NotFound();

		var employeeId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
		if (ticket.AssignedEmployeeId != employeeId) return Forbid();

		ticket.Status = TicketStatus.Closed;
		ticket.ClosedAt = DateTime.UtcNow;
		await _ticketRepo.UpdateAsync(ticket);

		_logger.LogInformation("Ticket {TicketId} closed by employee {EmployeeId}",
			id, employeeId);
		return Ok(new { message = "Ticket closed successfully" });
	}

	[HttpPost("{id}/comments")]
	public async Task<IActionResult> AddComment(int id, [FromBody] string content)
	{
		var ticket = await _ticketRepo.GetByIdAsync(id);
		if (ticket == null) return NotFound();

		var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

		var comment = new TicketComment
		{
			TicketId = id,
			UserId = userId,
			Content = content
		};

		await _commentRepo.AddAsync(comment);
		return Ok(new { message = "Comment added" });
	}

	private static TicketDto MapToDto(Ticket t) => new()
	{
		Id = t.Id,
		Title = t.Title,
		ProblemDescription = t.ProblemDescription,
		Status = t.Status.ToString(),
		CreatedAt = t.CreatedAt,
		UpdatedAt = t.UpdatedAt,
		ClosedAt = t.ClosedAt,
		ClientName = t.Client?.FullName ?? "",
		ClientId = t.ClientId,
		ProductName = t.Product?.Name ?? "",
		ProductId = t.ProductId,
		AssignedEmployeeName = t.AssignedEmployee?.FullName,
		AssignedEmployeeId = t.AssignedEmployeeId,
		Comments = t.Comments?.Select(c => new CommentDto
		{
			Id = c.Id,
			Content = c.Content,
			AuthorName = c.User?.FullName ?? "",
			AuthorType = c.User?.UserType.ToString() ?? "",
			CreatedAt = c.CreatedAt
		}).ToList() ?? new(),
		Attachments = t.Attachments?.Select(a => a.FilePath).ToList() ?? new()
	};
}