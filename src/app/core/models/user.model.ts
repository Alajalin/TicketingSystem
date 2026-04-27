 export interface User {
  id: number;
  fullName: string;
  email: string;
  mobileNumber: string;
  imagePath?: string;
  dateOfBirth: string;
  address?: string;
  userType: string;
  isActive: boolean;
  createdAt: string;
  ticketCount: number;
}

export interface RegisterEmployeeRequest {
  fullName: string;
  email: string;
  mobileNumber: string;
  password: string;
  dateOfBirth: string;
  address?: string;
}

export interface RegisterClientRequest {
  fullName: string;
  email: string;
  mobileNumber: string;
  password: string;
  dateOfBirth: string;
  address?: string;
}
