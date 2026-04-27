export interface LoginRequest {
  emailOrUsername: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  fullName: string;
  email: string;
  userType: string;
  userId: number;
}