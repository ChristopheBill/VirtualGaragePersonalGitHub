import { Router } from "express";
import { z } from "zod";
import { prisma } from "../db.js";
import { requireAdmin, requireAuth } from "../middleware/auth.js";
const router = Router();
const userInput = z.object({
    firstName: z.string().trim().min(1).max(50),
    lastName: z.string().trim().min(1).max(50),
    email: z.string().email().max(200),
});
const idSchema = z.string().uuid();
function userResponse(user) {
    return {
        id: user.id,
        firstName: user.firstName,
        lastName: user.lastName,
        email: user.email,
        vehicles: user.vehicles?.map((vehicle) => ({
            id: vehicle.id,
            brand: vehicle.brand,
            model: vehicle.model,
            manufactureDate: vehicle.manufactureDate.toISOString(),
        })),
    };
}
router.use(requireAuth, requireAdmin);
router.get("/", async (_request, response) => {
    const users = await prisma.user.findMany({ include: { vehicles: true } });
    response.json(users.map(userResponse));
});
router.get("/:id", async (request, response) => {
    const id = idSchema.safeParse(request.params.id);
    if (!id.success) {
        response.status(400).json({ error: "Invalid user id" });
        return;
    }
    const user = await prisma.user.findUnique({ where: { id: id.data }, include: { vehicles: true } });
    if (!user) {
        response.status(404).end();
        return;
    }
    response.json(userResponse(user));
});
router.post("/", async (request, response) => {
    const input = userInput.safeParse(request.body);
    if (!input.success) {
        response.status(400).json({ error: "Invalid user", details: input.error.flatten() });
        return;
    }
    const user = await prisma.user.create({
        data: { ...input.data, birthDay: new Date(), passwordHash: "", userRole: "user" },
        include: { vehicles: true },
    });
    response.json(userResponse(user));
});
router.delete("/:id", async (request, response) => {
    const id = idSchema.safeParse(request.params.id);
    if (!id.success) {
        response.status(400).json({ error: "Invalid user id" });
        return;
    }
    await prisma.user.deleteMany({ where: { id: id.data } });
    response.status(204).end();
});
export default router;
