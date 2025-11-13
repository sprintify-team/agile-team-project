export interface AuthResponse {
  token: string;
  expiration: string; // ISO datetime
  refreshToken: string;
  refreshTokenExpiration: string; // ISO datetime
}

export interface LoginRequest {
  userNameOrEmail: string;
  password: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  password: string;
}

export type RefreshRequest = {
  refreshToken: string;
};

export interface LogoutRequest {
  refreshToken: string;
}

// Backend'de JwtTokenService içinde üretilen claims’e göre
export type TokenPayload = {
  sub: string;               // user ID
  userName?: string;
  email?: string;
  name?: string;
  role?: string | string[];
  exp: number;               // epoch time (UTC seconds)
};

export type AuthUser = {
  id: string;                // from 'sub'
  userName?: string;
  email?: string;
  fullName?: string;
  roles: string[];
};

export type AuthState = {
  user: AuthUser | null;
  accessToken: string | null;
  refreshToken: string | null;
  isAuthenticated: boolean;
};


