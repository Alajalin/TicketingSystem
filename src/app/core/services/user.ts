import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User, RegisterEmployeeRequest } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'https://localhost:7203/api';

  constructor(private http: HttpClient) {}

  getAllEmployees(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/Users/employees`);
  }

  getAllClients(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/Users/clients`);
  }

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/Users/${id}`);
  }

  getProfile(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/Users/profile`);
  }

  addEmployee(data: RegisterEmployeeRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/Users/register-employee`, data);
  }

  toggleUserStatus(id: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/Users/${id}/toggle-status`, {});
  }
}