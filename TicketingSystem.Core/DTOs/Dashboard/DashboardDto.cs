using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Core.DTOs.Dashboard;

public class DashboardDto
{
	public int TotalTickets { get; set; }
	public int NewTickets { get; set; }
	public int AssignedTickets { get; set; }
	public int InProgressTickets { get; set; }
	public int ClosedTickets { get; set; }
	public int TotalClients { get; set; }
	public int TotalEmployees { get; set; }
	public List<EmployeeProductivityDto> TopEmployees { get; set; } = new();
	public List<StatusChartDto> StatusChart { get; set; } = new();
}

public class EmployeeProductivityDto
{
	public string EmployeeName { get; set; } = string.Empty;
	public int ClosedTickets { get; set; }
}

public class StatusChartDto
{
	public string Status { get; set; } = string.Empty;
	public int Count { get; set; }
}
