import "dotenv/config";
import { z } from "zod";
const configSchema = z.object({
    PORT: z.coerce.number().int().positive().default(3000),
    DATABASE_URL: z.string().min(1),
    OIDC_AUTHORITY: z.string().url(),
    OIDC_AUDIENCE: z.string().optional(),
    FRONTEND_ORIGIN: z.string().url().default("http://localhost:5173"),
});
export const config = configSchema.parse({
    PORT: process.env.PORT,
    DATABASE_URL: process.env.DATABASE_URL,
    OIDC_AUTHORITY: process.env.OIDC_AUTHORITY,
    OIDC_AUDIENCE: process.env.OIDC_AUDIENCE || undefined,
    FRONTEND_ORIGIN: process.env.FRONTEND_ORIGIN,
});
