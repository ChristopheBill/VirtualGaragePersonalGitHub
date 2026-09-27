import { app } from "./app.js";
import { config } from "./config.js";
import { prisma } from "./db.js";
const server = app.listen(config.PORT, () => {
    console.log(`VirtualGarage Node API listening on port ${config.PORT}`);
});
async function shutdown() {
    server.close();
    await prisma.$disconnect();
}
process.on("SIGINT", shutdown);
process.on("SIGTERM", shutdown);
