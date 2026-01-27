import api from "./axios";

export interface CreatePaymentIntentRequest {
  amount: number; // in cents
  email: string;
  currency: string;
}

export interface CreatePaymentIntentResponse {
  clientSecret: string;
  paymentIntentId: string;
  amount: number;
  currency: string;
}

export interface ConfirmPaymentRequest {
  paymentIntentId: string;
  clientSecret: string;
  card: {
    number: string;
    exp_month: string;
    exp_year: string;
    cvc: string;
  };
}

export interface ConfirmPaymentResponse {
  status: string;
  paymentIntentId: string;
  amount: number;
}

/**
 * Create a payment intent for donation
 */
export async function createPaymentIntent(
  data: CreatePaymentIntentRequest
): Promise<CreatePaymentIntentResponse> {
  const response = await api.post<CreatePaymentIntentResponse>(
    "/donations/create-payment-intent",
    data
  );
  return response.data;
}

/**
 * Confirm the payment (simulated or real Stripe)
 */
export async function confirmPayment(
  data: ConfirmPaymentRequest
): Promise<ConfirmPaymentResponse> {
  const response = await api.post<ConfirmPaymentResponse>(
    "/donations/confirm-payment",
    data
  );
  return response.data;
}
