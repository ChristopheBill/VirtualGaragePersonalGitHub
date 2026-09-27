import "dotenv/config";
import { z } from "zod";
const configSchema = z.object({
    PORT: z.coerce.number().int().positive().default(3000),
    DATABASE_URL: z.string().min(1),
    JWT_SECRET: z.string().min(32),
    FRONTEND_ORIGIN: z.string().url().default("http://localhost:5173"),
});
export const config = configSchema.parse({
    PORT: process.env.PORT,
    DATABASE_URL: process.env.DATABASE_URL,
    JWT_SECRET: process.env.JWT_SECRET,
    FRONTEND_ORIGIN: process.env.FRONTEND_ORIGIN,
});
