# Donation Feature - Setup Guide

## What's Been Created

### Frontend (React/TypeScript)
- **[DonationPage.tsx](virtualgarage-frontend/src/pages/DonationPage.tsx)** - Main donation form with:
  - Preset donation amounts ($5, $10, $25, $50, $100)
  - Custom amount input
  - Card payment form (test card simulation)
  - Beautiful UI with Tailwind CSS
  - Success/error messaging

- **[donations.ts](virtualgarage-frontend/src/api/donations.ts)** - API client with:
  - `createPaymentIntent()` - Backend call to create payment intent
  - `confirmPayment()` - Backend call to confirm payment

- **Route Added** - `/donate` accessible from any protected route

### Backend (.NET)
- **[DonationRequestContract.cs](VirtualGarage.Contracts/RequestContracts/DonationRequestContract.cs)** - Request DTOs
- **[DonationResponseContract.cs](VirtualGarage.Contracts/ResponseContracts/DonationResponseContract.cs)** - Response DTOs
- **[Donation.cs](VirtualGarage.Persistence.Entities/Donation.cs)** - Entity model
- **[IDonationRepository.cs](VirtualGarage.Persistence/Interfaces/IDonationRepository.cs)** - Repository interface
- **[DonationRepository.cs](VirtualGarage.Persistence/DonationRepository.cs)** - Repository implementation
- **[IDonationService.cs](VirtualGarage.Domain.Services/Interfaces/IDonationService.cs)** - Service interface
- **[DonationService.cs](VirtualGarage.Domain.Services/DonationService.cs)** - Service implementation with payment logic
- **[DonationsController.cs](VirtualGarage.Api/Controllers/DonationsController.cs)** - API endpoints
- **[20260127_AddDonationsTable.cs](VirtualGarage.Persistence/Migrations/20260127_AddDonationsTable.cs)** - Database migration

## Quick Start

### 1. Frontend Setup (Optional - Already Configured)
Install Stripe packages when ready for production:
```bash
cd virtualgarage-frontend
npm install @stripe/react-stripe-js @stripe/stripe-js
```

### 2. Backend Setup
Register services in Program.cs (Already Done ✓):
- `IDonationService` / `DonationService`
- `IDonationRepository` / `DonationRepository`

### 3. Database Migration
Run the migration to add Donations table:
```bash
# Navigate to VirtualGarage.Api directory
dotnet ef database update
```

### 4. Test the Feature
1. Start your API server
2. Start your frontend dev server
3. Login to your app
4. Navigate to `/donate`
5. Try a donation with test card: **4242 4242 4242 4242**
6. Expiry: **12/25**, CVC: **123**

## Test Card Numbers (Demo Mode)
- **4242 4242 4242 4242** - Success
- **5555 5555 5555 4444** - Also works in demo
- Any other 16-digit number starting with 4 or 5 - Will work

## Current Features

### ✅ Working Now
- **Payment Intent Creation** - Backend generates payment intent ID and secret
- **Payment Confirmation** - Validates and records donations
- **Test Card Simulation** - Accept demo card numbers
- **Database Storage** - Donations are saved with user tracking
- **User Authorization** - Only authenticated users can donate
- **Beautiful UI** - Responsive donation page with Tailwind

### 📝 Optional Enhancements (For Production)

#### Real Stripe Integration
1. Add Stripe NuGet package:
   ```bash
   dotnet add package Stripe.net
   ```

2. Add Stripe API key to Key Vault:
   ```
   Stripe-ApiKey: sk_live_xxx
   Stripe-PublishableKey: pk_live_xxx
   ```

3. Update `DonationService.cs` to use actual Stripe SDK:
   ```csharp
   var paymentService = new PaymentIntentService();
   var intent = await paymentService.CreateAsync(new PaymentIntentCreateOptions
   {
       Amount = request.Amount,
       Currency = request.Currency,
       PaymentMethodTypes = new List<string> { "card" }
   });
   ```

4. Update frontend to use Stripe Elements or Payment Element

#### Webhook Handling
- Set up Stripe webhooks for async payment updates
- Handle `payment_intent.succeeded` and `payment_intent.payment_failed` events

#### Email Notifications
- Send confirmation email to donor
- Track donation for analytics

#### Donation Management Page
- Admin dashboard to view all donations
- Donor history page for users

## File Structure
```
Frontend:
  src/
    ├── pages/DonationPage.tsx
    └── api/donations.ts

Backend:
  VirtualGarage.Contracts/
    ├── RequestContracts/DonationRequestContract.cs
    └── ResponseContracts/DonationResponseContract.cs
  
  VirtualGarage.Persistence.Entities/
    ├── Donation.cs
    └── VirtualGarageDbContext.cs (updated)
  
  VirtualGarage.Persistence/
    ├── DonationRepository.cs
    ├── Interfaces/IDonationRepository.cs
    └── Migrations/20260127_AddDonationsTable.cs
  
  VirtualGarage.Domain.Services/
    ├── DonationService.cs
    └── Interfaces/IDonationService.cs
  
  VirtualGarage.Api/
    ├── Controllers/DonationsController.cs
    └── Program.cs (updated)
```

## API Endpoints

### POST /api/donations/create-payment-intent
Creates a payment intent for a donation.

**Request:**
```json
{
  "amount": 2500,
  "email": "user@example.com",
  "currency": "usd"
}
```

**Response:**
```json
{
  "clientSecret": "pi_xxx_secret_yyy",
  "paymentIntentId": "pi_xxx",
  "amount": 2500,
  "currency": "usd"
}
```

### POST /api/donations/confirm-payment
Confirms the payment and records the donation.

**Request:**
```json
{
  "paymentIntentId": "pi_xxx",
  "clientSecret": "pi_xxx_secret_yyy",
  "card": {
    "number": "4242424242424242",
    "exp_month": "12",
    "exp_year": "25",
    "cvc": "123"
  }
}
```

**Response:**
```json
{
  "status": "succeeded",
  "paymentIntentId": "pi_xxx",
  "amount": 2500
}
```

## Next Steps

1. ✅ Verify all files compile
2. ✅ Run database migration: `dotnet ef database update`
3. ✅ Test with frontend at `/donate`
4. 📝 (Optional) Integrate real Stripe
5. 📝 (Optional) Add admin donation dashboard
6. 📝 (Optional) Add email notifications

## Notes
- Currently uses **simulated payments** - no real charges
- In production, add Stripe SDK and real payment processing
- Card details are validated client-side for demo purposes
- Donations are stored with user tracking for audit trail
