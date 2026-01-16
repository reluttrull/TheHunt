export interface RegisterRequest {
  email: string;
  userName: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface TokenResponse {
  accessToken: string;
  refreshToken: string;
}

export interface UserResponse {
  id: string;
  email: string;
  userName: string;
  joinedDate: Date;
}