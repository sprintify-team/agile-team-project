import apiClient from "./apiClient";
import type {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
  RefreshRequest,
  LogoutRequest,
} from "../types/auth";

export const authService = {
  // LOGIN
  async login(data: LoginRequest): Promise<AuthResponse> {
    const res = await apiClient.post<AuthResponse>("/Auth/login", data);
    return res.data;
  },

  // REGISTER (rol istemiyoruz, backend default olarak Student veriyor)
  async register(data: RegisterRequest): Promise<AuthResponse> {
    const res = await apiClient.post<AuthResponse>("/Auth/register", data);
    return res.data;
  },

  // REFRESH
  async refresh(): Promise<AuthResponse> {
    const refreshToken = localStorage.getItem("refreshToken");
    if (!refreshToken) throw new Error("No refresh token found");

    const body: RefreshRequest = { refreshToken };
    const res = await apiClient.post<AuthResponse>("/Auth/refresh", body);
    return res.data;
  },

  // LOGOUT
  async logout(): Promise<void> {
    const refreshToken = localStorage.getItem("refreshToken");
    if (!refreshToken) return;

    const body: LogoutRequest = { refreshToken };
    await apiClient.post("/Auth/logout", body);
  },
};
