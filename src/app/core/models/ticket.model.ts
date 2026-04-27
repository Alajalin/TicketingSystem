export interface Ticket {
  id: number;
  title: string;
  problemDescription: string;
  status: string;
  createdAt: string;
  updatedAt?: string;
  closedAt?: string;
  clientName: string;
  clientId: number;
  productName: string;
  productId: number;
  assignedEmployeeName?: string;
  assignedEmployeeId?: number;
  comments: Comment[];
  attachments: string[];
}

export interface Comment {
  id: number;
  content: string;
  authorName: string;
  authorType: string;
  createdAt: string;
}

export interface CreateTicketRequest {
  title: string;
  problemDescription: string;
  productId: number;
}

export interface AssignTicketRequest {
  employeeId: number;
}

export interface Product {
  id: number;
  name: string;
  description?: string;
  isActive: boolean;
}

export interface DashboardData {
  totalTickets: number;
  newTickets: number;
  assignedTickets: number;
  inProgressTickets: number;
  closedTickets: number;
  totalClients: number;
  totalEmployees: number;
  topEmployees: TopEmployee[];
  statusChart: StatusChart[];
}

export interface TopEmployee {
  employeeName: string;
  closedTickets: number;
}

export interface StatusChart {
  status: string;
  count: number;
} 
