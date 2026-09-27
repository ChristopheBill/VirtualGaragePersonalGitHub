import { createContext, useContext, useMemo, useState, type ReactNode } from "react";

export interface AuthUser {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
}

interface AuthContextValue {
  user: AuthUser | null;
  accessToken: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: Error | null;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);
const userKey = "virtualgarage.user";
const tokenKey = "virtualgarage.token";

function readUser(): AuthUser | null {
  const storedUser = localStorage.getItem(userKey);
  if (!storedUser) return null;
  try {
    return JSON.parse(storedUser) as AuthUser;
  } catch {
    localStorage.removeItem(userKey);
    return null;
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthUser | null>(readUser);
  const [accessToken, setAccessToken] = useState<string | null>(() => localStorage.getItem(tokenKey));
  const [error, setError] = useState<Error | null>(null);

  const value = useMemo<AuthContextValue>(() => ({
    user,
    accessToken,
    isAuthenticated: Boolean(user && accessToken),
    isLoading: false,
    error,
    async login(email, password) {
      setError(null);
      const response = await fetch(`${import.meta.env.VITE_API_BASE_URL || "http://localhost:3000/api"}/auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });
      const body = await response.json() as { error?: string; token?: string; user?: AuthUser };
      if (!response.ok || !body.token || !body.user) {
        const loginError = new Error(body.error || "Login failed");
        setError(loginError);
        throw loginError;
      }
      localStorage.setItem(tokenKey, body.token);
      localStorage.setItem(userKey, JSON.stringify(body.user));
      setAccessToken(body.token);
      setUser(body.user);
    },
    logout() {
      localStorage.removeItem(tokenKey);
      localStorage.removeItem(userKey);
      setAccessToken(null);
      setUser(null);
    },
  }), [accessToken, error, user]);

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth must be used within AuthProvider");
  return context;
}