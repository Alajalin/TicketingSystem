import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth';
import { LoginRequest } from '../../../core/models/auth.model';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  loginData: LoginRequest = {
    emailOrUsername: '',
    password: ''
  };

  errorMessage = '';
  isLoading = false;

  constructor(private authService: AuthService, private router: Router) {}

  onLogin(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.loginData).subscribe({
      next: (response) => {
        this.authService.saveToken(response);
        this.redirectUser(response.userType);
      },
      error: (err) => {
        this.errorMessage = 'Invalid email or password';
        this.isLoading = false;
      }
    });
  }

  redirectUser(userType: string): void {
    if (userType === 'Manager') {
      this.router.navigate(['/dashboard']);
    } else if (userType === 'SupportEmployee') {
      this.router.navigate(['/tickets']);
    } else {
      this.router.navigate(['/tickets']);
    }
  }
}