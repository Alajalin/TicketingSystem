using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Core.Enums;

public enum TicketStatus
{
	New = 1,
	Assigned = 2,
	InProgress = 3,
	PendingClientConfirmation = 4,
	Closed = 5
}