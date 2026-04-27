import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequest, AuthResponse } from '../models/auth.model';
import { RegisterClientRequest } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7203/api';

  constructor(private http: HttpClient) {}

  login(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Auth/login`, data);
  }

  registerClient(data: RegisterClientRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/Auth/register-client`, data);
  }

  saveToken(response: AuthResponse): void {
    localStorage.setItem('token', response.token);
    localStorage.setItem('userType', response.userType);
    localStorage.setItem('fullName', response.fullName);
    localStorage.setItem('userId', response.userId.toString());
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUserType(): string | null {
    return localStorage.getItem('userType');
  }

  getFullName(): string | null {
    return localStorage.getItem('fullName');
  }

  getUserId(): number {
    return Number(localStorage.getItem('userId'));
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    localStorage.clear();
  }
}