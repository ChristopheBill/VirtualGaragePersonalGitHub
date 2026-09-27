import express from "express";
import cors from "cors";
import { config } from "./config.js";
import vehiclesRouter from "./routes/vehicles.js";
import usersRouter from "./routes/users.js";
import { errorHandler } from "./middleware/errors.js";
export const app = express();
app.use(cors({ origin: config.FRONTEND_ORIGIN }));
app.use(express.json());
app.get("/health", (_request, response) => {
    response.json({ status: "ok" });
});
app.use("/api/vehicles", vehiclesRouter);
app.use("/api/users", usersRouter);
app.use(errorHandler);
