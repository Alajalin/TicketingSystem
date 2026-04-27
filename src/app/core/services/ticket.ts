import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Ticket, CreateTicketRequest, AssignTicketRequest, DashboardData } from '../models/ticket.model';

@Injectable({
  providedIn: 'root'
})
export class TicketService {

  private apiUrl = 'https://localhost:7203/api';

  constructor(private http: HttpClient) {}

  getAllTickets(status?: string, employeeId?: number, clientId?: number): Observable<Ticket[]> {
    let params = new HttpParams();
    if (status) params = params.set('status', status);
    if (employeeId) params = params.set('employeeId', employeeId);
    if (clientId) params = params.set('clientId', clientId);
    return this.http.get<Ticket[]>(`${this.apiUrl}/Tickets`, { params });
  }

  getMyTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.apiUrl}/Tickets/my`);
  }

  getAssignedTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.apiUrl}/Tickets/assigned`);
  }

  getTicketById(id: number): Observable<Ticket> {
    return this.http.get<Ticket>(`${this.apiUrl}/Tickets/${id}`);
  }

  createTicket(data: CreateTicketRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/Tickets`, data);
  }

  assignTicket(id: number, data: AssignTicketRequest): Observable<any> {
    return this.http.put(`${this.apiUrl}/Tickets/${id}/assign`, data);
  }

  updateStatus(id: number, status: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/Tickets/${id}/status`, { status });
  }

  addComment(id: number, content: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/Tickets/${id}/comments`, { content });
  }

  getDashboard(): Observable<DashboardData> {
    return this.http.get<DashboardData>(`${this.apiUrl}/Dashboard`);
  }
}