import type { NextFunction, Request, Response } from "express";
import { createRemoteJWKSet, jwtVerify } from "jose";
import { z } from "zod";
import { config } from "../config.js";
import type { AuthenticatedRequest } from "../types/auth.js";

const issuer = config.OIDC_AUTHORITY.replace(/\/$/, "");
const jwks = createRemoteJWKSet(new URL(`${issuer}/.well-known/openid-configuration/jwks`));

export async function requireAuth(request: Request, response: Response, next: NextFunction) {
  const authorization = request.header("authorization");
  const token = authorization?.startsWith("Bearer ") ? authorization.slice(7) : undefined;

  if (!token) {
    response.status(401).json({ error: "Bearer token is required" });
    return;
  }

  try {
    const { payload } = await jwtVerify(token, jwks, {
      issuer,
      ...(config.OIDC_AUDIENCE ? { audience: config.OIDC_AUDIENCE } : {}),
    });
    const subject = typeof payload.sub === "string" ? payload.sub : undefined;

    if (!subject || !z.string().uuid().safeParse(subject).success) {
      response.status(401).json({ error: "Token does not contain a subject" });
      return;
    }

    const roleClaim = payload.role ?? payload.roles;
    const roles = Array.isArray(roleClaim)
      ? roleClaim.filter((role): role is string => typeof role === "string")
      : typeof roleClaim === "string" ? [roleClaim] : [];

    (request as AuthenticatedRequest).user = {
      id: subject,
      roles,
      claims: payload as Record<string, unknown>,
    };
    next();
  } catch {
    response.status(401).json({ error: "Invalid access token" });
  }
}

export function requireAdmin(request: Request, response: Response, next: NextFunction) {
  const user = (request as AuthenticatedRequest).user;
  if (!user.roles.includes("Admin")) {
    response.status(403).json({ error: "Administrator role is required" });
    return;
  }
  next();
}
