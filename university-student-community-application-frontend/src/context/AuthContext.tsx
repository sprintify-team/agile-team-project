import React, { createContext, useEffect, useMemo, useState } from "react";
import type {
  AuthResponse,
  AuthState,
  LoginRequest,
  RegisterRequest,
  TokenPayload,
  AuthUser,
} from "../types/auth";
import { authService } from "../services/authService";

/* ----------------------- JWT Decode Helper ----------------------- */
function decodeToken(token: string): TokenPayload | null {
  try {
    const base64 = token.split(".")[1];
    const normalized = base64.replace(/-/g, "+").replace(/_/g, "/");
    const json = atob(normalized);
    return JSON.parse(json);
  } catch {
    return null;
  }
}

function extractRoles(payload: TokenPayload | null): string[] {
  if (!payload) return [];
  const r = payload.role;
  return !r ? [] : Array.isArray(r) ? r : [r];
}

function mapUserFromPayload(p: TokenPayload | null): AuthUser | null {
  if (!p) return null;
  return {
    id: p.sub,
    userName: p.userName,
    email: p.email,
    fullName: p.name,
    roles: extractRoles(p),
  };
}

function isExpired(payload: TokenPayload | null): boolean {
  if (!payload?.exp) return true;
  return Date.now() >= payload.exp * 1000;
}

/* ----------------------- LocalStorage Helpers ----------------------- */
const ACCESS = "accessToken";
const REFRESH = "refreshToken";

function loadTokens() {
  return {
    accessToken: localStorage.getItem(ACCESS),
    refreshToken: localStorage.getItem(REFRESH),
  };
}

function persistTokens(res: AuthResponse) {
  localStorage.setItem(ACCESS, res.token);
  localStorage.setItem(REFRESH, res.refreshToken);
}

function clearTokens() {
  localStorage.removeItem(ACCESS);
  localStorage.removeItem(REFRESH);
}

/* ----------------------- AuthContext Type ----------------------- */
type AuthContextValue = AuthState & {
  loading: boolean;
  login: (data: LoginRequest) => Promise<void>;
  register: (data: RegisterRequest) => Promise<void>;
  refresh: () => Promise<void>;
  logout: () => Promise<void>;
  hasRole: (role: string) => boolean;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

/* ----------------------- PROVIDER ----------------------- */
export const AuthProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
  const [{ user, accessToken, refreshToken, isAuthenticated }, setState] =
    useState<AuthState>({
      user: null,
      accessToken: null,
      refreshToken: null,
      isAuthenticated: false,
    });

  const [loading, setLoading] = useState(true);

  /* Şu anki giriş durumu → LocalStorage'dan yükle */
  useEffect(() => {
    const { accessToken: at, refreshToken: rt } = loadTokens();
    if (at) {
      const payload = decodeToken(at);
      if (payload && !isExpired(payload)) {
        setState({
          user: mapUserFromPayload(payload),
          accessToken: at,
          refreshToken: rt,
          isAuthenticated: true,
        });
      } else {
        clearTokens();
      }
    }
    setLoading(false);
  }, []);

  /* ACTIONS */
  const login = async (data: LoginRequest) => {
    const res = await authService.login(data);
    persistTokens(res);

    const payload = decodeToken(res.token);
    setState({
      user: mapUserFromPayload(payload),
      accessToken: res.token,
      refreshToken: res.refreshToken,
      isAuthenticated: true,
    });
  };

  const register = async (data: RegisterRequest) => {
    const res = await authService.register(data);
    persistTokens(res);

    const payload = decodeToken(res.token);
    setState({
      user: mapUserFromPayload(payload),
      accessToken: res.token,
      refreshToken: res.refreshToken,
      isAuthenticated: true,
    });
  };

  const refresh = async () => {
    const res = await authService.refresh();
    persistTokens(res);

    const payload = decodeToken(res.token);
    setState({
      user: mapUserFromPayload(payload),
      accessToken: res.token,
      refreshToken: res.refreshToken,
      isAuthenticated: true,
    });
  };

  const logout = async () => {
    try {
      await authService.logout();
    } finally {
      clearTokens();
      setState({
        user: null,
        accessToken: null,
        refreshToken: null,
        isAuthenticated: false,
      });
    }
  };

  const hasRole = (role: string) => {
    return user?.roles?.includes(role) ?? false;
  };

  const value = useMemo(
    () => ({
      user,
      accessToken,
      refreshToken,
      isAuthenticated,
      loading,
      login,
      register,
      refresh,
      logout,
      hasRole,
    }),
    [user, accessToken, refreshToken, isAuthenticated, loading]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export default AuthContext;
