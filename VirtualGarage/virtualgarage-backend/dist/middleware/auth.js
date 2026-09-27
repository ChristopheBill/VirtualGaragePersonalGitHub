import { jwtVerify } from "jose";
import { config } from "../config.js";
const secret = new TextEncoder().encode(config.JWT_SECRET);
export async function requireAuth(request, response, next) {
    const authorization = request.header("authorization");
    const token = authorization?.startsWith("Bearer ") ? authorization.slice(7) : undefined;
    if (!token) {
        response.status(401).json({ error: "Bearer token is required" });
        return;
    }
    try {
        const { payload } = await jwtVerify(token, secret);
        const subject = typeof payload.sub === "string" ? payload.sub : undefined;
        if (!subject) {
            response.status(401).json({ error: "Token does not contain a subject" });
            return;
        }
        const roleClaim = payload.role ?? payload.roles;
        const roles = Array.isArray(roleClaim)
            ? roleClaim.filter((role) => typeof role === "string")
            : typeof roleClaim === "string" ? [roleClaim] : [];
        request.user = {
            id: subject,
            roles,
            claims: payload,
        };
        next();
    }
    catch {
        response.status(401).json({ error: "Invalid access token" });
    }
}
export function requireAdmin(request, response, next) {
    const user = request.user;
    if (!user.roles.includes("admin")) {
        response.status(403).json({ error: "Administrator role is required" });
        return;
    }
    next();
}
