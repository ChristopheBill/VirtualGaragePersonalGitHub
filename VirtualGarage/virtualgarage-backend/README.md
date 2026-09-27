# VirtualGarage Node backend

TypeScript/Express backend for the VirtualGarage frontend. It uses one MySQL database through Prisma and local JWT authentication.

## Local setup

1. Copy `.env.example` to `.env`.
2. Set `DATABASE_URL` to a MySQL connection string.
3. Set `JWT_SECRET` to a random value of at least 32 characters.
4. Run `npm install` and `npm run prisma:generate`.
5. Start the API with `npm run dev`.

The API includes `/api/auth/register`, `/api/auth/login`, vehicle CRUD, and admin-protected user routes. Run `npm run prisma:generate` followed by `npm run prisma:migrate` after configuring MySQL.
