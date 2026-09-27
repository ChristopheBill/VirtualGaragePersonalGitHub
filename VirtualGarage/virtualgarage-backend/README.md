# VirtualGarage Node backend

TypeScript/Express replacement for `VirtualGarage.Api`. It keeps the existing API base paths and validates access tokens issued by the existing IdentityServer.

## Local setup

1. Copy `.env.example` to `.env`.
2. Set `DATABASE_URL` to the existing SQL Server database connection string.
3. Set `OIDC_AUTHORITY` to the IdentityServer authority.
4. Run `npm install` and `npm run prisma:generate`.
5. Start the API with `npm run dev`.

The current implementation includes `GET/POST/PUT/DELETE /api/vehicles` and `GET /api/vehicles/mine`. The remaining adapters will be added without changing the frontend contract.
