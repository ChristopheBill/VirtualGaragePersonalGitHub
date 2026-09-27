import { randomBytes, scrypt as scryptCallback, timingSafeEqual } from "node:crypto";
import { promisify } from "node:util";
import { Router } from "express";
import { SignJWT } from "jose";
import { z } from "zod";
import { config } from "../config.js";
import { prisma } from "../db.js";

const router = Router();
const scrypt = promisify(scryptCallback);
const secret = new TextEncoder().encode(config.JWT_SECRET);
const credentialsSchema = z.object({
  email: z.string().trim().email().max(200),
  password: z.string().min(8).max(128),
});
const registerSchema = credentialsSchema.extend({
  firstName: z.string().trim().min(1).max(50),
  lastName: z.string().trim().min(1).max(50),
  birthDay: z.coerce.date().optional(),
});

async function hashPassword(password: string): Promise<string> {
  const salt = randomBytes(16);
  const derivedKey = await scrypt(password, salt, 64) as Buffer;
  return `${salt.toString("hex")}:${derivedKey.toString("hex")}`;
}

async function verifyPassword(password: string, storedHash: string): Promise<boolean> {
  const [saltHex, hashHex] = storedHash.split(":");
  if (!saltHex || !hashHex) return false;

  const derivedKey = await scrypt(password, Buffer.from(saltHex, "hex"), 64) as Buffer;
  const expectedKey = Buffer.from(hashHex, "hex");
  return expectedKey.length === derivedKey.length && timingSafeEqual(expectedKey, derivedKey);
}

async function createToken(user: { id: string; email: string; userRole: string }): Promise<string> {
  return new SignJWT({ email: user.email, role: user.userRole })
    .setProtectedHeader({ alg: "HS256" })
    .setSubject(user.id)
    .setIssuedAt()
    .setExpirationTime("2h")
    .sign(secret);
}

function publicUser(user: { id: string; email: string; firstName: string; lastName: string; userRole: string }) {
  return {
    id: user.id,
    email: user.email,
    firstName: user.firstName,
    lastName: user.lastName,
    role: user.userRole,
  };
}

router.post("/register", async (request, response) => {
  const input = registerSchema.safeParse(request.body);
  if (!input.success) {
    response.status(400).json({ error: "Invalid registration data", details: input.error.flatten() });
    return;
  }

  const existingUser = await prisma.user.findUnique({ where: { email: input.data.email } });
  if (existingUser) {
    response.status(409).json({ error: "An account with this email already exists" });
    return;
  }

  const user = await prisma.user.create({
    data: {
      email: input.data.email,
      passwordHash: await hashPassword(input.data.password),
      firstName: input.data.firstName,
      lastName: input.data.lastName,
      birthDay: input.data.birthDay ?? new Date(),
      userRole: "user",
    },
  });

  response.status(201).json({ user: publicUser(user), token: await createToken(user) });
});

router.post("/login", async (request, response) => {
  const input = credentialsSchema.safeParse(request.body);
  if (!input.success) {
    response.status(400).json({ error: "Invalid login data", details: input.error.flatten() });
    return;
  }

  const user = await prisma.user.findUnique({ where: { email: input.data.email } });
  if (!user || !(await verifyPassword(input.data.password, user.passwordHash))) {
    response.status(401).json({ error: "Invalid email or password" });
    return;
  }

  response.json({ user: publicUser(user), token: await createToken(user) });
});

export default router;