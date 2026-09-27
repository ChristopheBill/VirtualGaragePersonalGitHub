import { Router } from "express";
import { z } from "zod";
import { prisma } from "../db.js";
import { requireAuth } from "../middleware/auth.js";
const router = Router();
const vehicleInput = z.object({
    brand: z.string().trim().min(1).max(50),
    model: z.string().trim().min(1).max(50),
    manufactureDate: z.coerce.date(),
});
const idSchema = z.string().uuid();
function vehicleResponse(vehicle) {
    return {
        id: vehicle.id,
        brand: vehicle.brand,
        model: vehicle.model,
        manufactureDate: vehicle.manufactureDate.toISOString(),
    };
}
router.use(requireAuth);
router.get("/mine", async (request, response) => {
    const userId = request.user.id;
    const vehicles = await prisma.vehicle.findMany({ where: { userId }, orderBy: { brand: "asc" } });
    response.json(vehicles.map(vehicleResponse));
});
router.get("/:id", async (request, response) => {
    const id = idSchema.safeParse(request.params.id);
    if (!id.success) {
        response.status(400).json({ error: "Invalid vehicle id" });
        return;
    }
    const vehicle = await prisma.vehicle.findUnique({ where: { id: id.data } });
    if (!vehicle) {
        response.status(404).end();
        return;
    }
    response.json(vehicleResponse(vehicle));
});
router.post("/", async (request, response) => {
    const input = vehicleInput.safeParse(request.body);
    if (!input.success) {
        response.status(400).json({ error: "Invalid vehicle", details: input.error.flatten() });
        return;
    }
    const userId = request.user.id;
    const vehicle = await prisma.vehicle.create({ data: { ...input.data, userId } });
    response.status(200).json(vehicleResponse(vehicle));
});
router.put("/:id", async (request, response) => {
    const id = idSchema.safeParse(request.params.id);
    const input = vehicleInput.safeParse(request.body);
    if (!id.success || !input.success) {
        response.status(400).json({ error: "Invalid vehicle" });
        return;
    }
    const userId = request.user.id;
    const existing = await prisma.vehicle.findFirst({ where: { id: id.data, userId } });
    if (!existing) {
        response.status(404).end();
        return;
    }
    const vehicle = await prisma.vehicle.update({ where: { id: id.data }, data: input.data });
    response.json(vehicleResponse(vehicle));
});
router.delete("/:id", async (request, response) => {
    const id = idSchema.safeParse(request.params.id);
    if (!id.success) {
        response.status(400).json({ error: "Invalid vehicle id" });
        return;
    }
    const userId = request.user.id;
    const existing = await prisma.vehicle.findFirst({ where: { id: id.data, userId } });
    if (!existing) {
        response.status(404).end();
        return;
    }
    await prisma.vehicle.delete({ where: { id: id.data } });
    response.status(204).end();
});
export default router;
