import type { Request } from "express";

export interface AuthenticatedUser {
  id: string;
  roles: string[];
  claims: Record<string, unknown>;
}

export type AuthenticatedRequest = Request & {
  user: AuthenticatedUser;
};
