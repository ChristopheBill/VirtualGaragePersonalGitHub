import { useState } from "react";
import Loading from "../components/common/Loading";
import { useAuth } from "../providers/AuthProvider";
import { createPaymentIntent, confirmPayment } from "../api/donations";

const PRESET_AMOUNTS = [5, 10, 25, 50, 100];

export default function DonationPage() {
  const auth = useAuth();
  const [amount, setAmount] = useState<number | "">(25);
  const [customAmount, setCustomAmount] = useState("");
  const [cardNumber, setCardNumber] = useState("");
  const [cardExpiry, setCardExpiry] = useState("");
  const [cardCvc, setCardCvc] = useState("");
  const [email, setEmail] = useState(auth.user?.email || "");
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<{
    type: "success" | "error";
    text: string;
  } | null>(null);

  const finalAmount = customAmount ? parseFloat(customAmount) : amount;

  const handlePresetAmount = (preset: number) => {
    setAmount(preset);
    setCustomAmount("");
  };

  const handleCustomAmount = (value: string) => {
    setCustomAmount(value);
    setAmount("");
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setMessage(null);
    setLoading(true);

    try {
      if (!finalAmount || finalAmount <= 0) {
        setMessage({ type: "error", text: "Please enter a valid amount" });
        setLoading(false);
        return;
      }

      if (!email) {
        setMessage({ type: "error", text: "Email is required" });
        setLoading(false);
        return;
      }

      // For demo: validate card format (basic check)
      if (cardNumber.replace(/\s/g, "").length < 15) {
        setMessage({
          type: "error",
          text: "Please enter a valid card number",
        });
        setLoading(false);
        return;
      }

      // Step 1: Create payment intent
      const { clientSecret, paymentIntentId } = await createPaymentIntent({
        amount: Math.round(finalAmount * 100), // Convert to cents
        email,
        currency: "usd",
      });

      // Step 2: Confirm payment (simulated)
      await confirmPayment({
        paymentIntentId,
        clientSecret,
        card: {
          number: cardNumber.replace(/\s/g, ""),
          exp_month: cardExpiry.split("/")[0],
          exp_year: cardExpiry.split("/")[1],
          cvc: cardCvc,
        },
      });

      setMessage({
        type: "success",
        text: `✓ Donation of $${finalAmount.toFixed(2)} successful! Thank you for your support.`,
      });

      // Reset form
      setAmount(25);
      setCustomAmount("");
      setCardNumber("");
      setCardExpiry("");
      setCardCvc("");
    } catch (error) {
      setMessage({
        type: "error",
        text:
          error instanceof Error
            ? error.message
            : "Payment failed. Please try again.",
      });
    } finally {
      setLoading(false);
    }
  };

  const handleCardNumberChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    let value = e.target.value.replace(/\s/g, "").replace(/\D/g, "");
    // Add spaces every 4 digits
    value = value.replace(/(\d{4})/g, "$1 ").trim();
    setCardNumber(value);
  };

  const handleExpiryChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    let value = e.target.value.replace(/\D/g, "");
    if (value.length >= 2) {
      value = value.slice(0, 2) + "/" + value.slice(2, 4);
    }
    setCardExpiry(value);
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 dark:from-neutral-900 dark:to-neutral-800 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-2xl mx-auto">
        {/* Header */}
        <div className="text-center mb-12">
          <h1 className="text-4xl font-bold text-neutral-900 dark:text-white mb-4">
            Support Virtual Garage
          </h1>
          <p className="text-lg text-neutral-600 dark:text-neutral-300">
            Your donation helps us build better tools for vehicle management
          </p>
        </div>

        {/* Message */}
        {message && (
          <div
            className={`mb-6 p-4 rounded-lg ${
              message.type === "success"
                ? "bg-green-50 dark:bg-green-900/30 text-green-800 dark:text-green-200 border border-green-200 dark:border-green-700"
                : "bg-red-50 dark:bg-red-900/30 text-red-800 dark:text-red-200 border border-red-200 dark:border-red-700"
            }`}
          >
            {message.text}
          </div>
        )}

        {/* Form Card */}
        <div className="bg-white dark:bg-neutral-800 rounded-2xl shadow-xl p-8">
          <form onSubmit={handleSubmit} className="space-y-8">
            {/* Amount Selection */}
            <div>
              <label className="block text-sm font-semibold text-neutral-900 dark:text-white mb-4">
                Donation Amount
              </label>
              <div className="grid grid-cols-5 gap-2 mb-4">
                {PRESET_AMOUNTS.map((preset) => (
                  <button
                    key={preset}
                    type="button"
                    onClick={() => handlePresetAmount(preset)}
                    className={`py-2 px-3 rounded-lg font-semibold transition-all ${
                      amount === preset
                        ? "bg-blue-600 text-white shadow-lg"
                        : "bg-neutral-100 dark:bg-neutral-700 text-neutral-900 dark:text-white hover:bg-neutral-200 dark:hover:bg-neutral-600"
                    }`}
                  >
                    ${preset}
                  </button>
                ))}
              </div>

              <div className="relative">
                <span className="absolute left-3 top-3 text-neutral-500 dark:text-neutral-400">
                  $
                </span>
                <input
                  type="number"
                  placeholder="Custom amount"
                  value={customAmount}
                  onChange={(e) => handleCustomAmount(e.target.value)}
                  step="0.01"
                  min="0"
                  className="w-full pl-8 pr-4 py-3 rounded-lg border border-neutral-200 dark:border-neutral-600 bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white placeholder-neutral-400 dark:placeholder-neutral-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
            </div>

            {/* Email */}
            <div>
              <label className="block text-sm font-semibold text-neutral-900 dark:text-white mb-2">
                Email
              </label>
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="w-full px-4 py-3 rounded-lg border border-neutral-200 dark:border-neutral-600 bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white placeholder-neutral-400 dark:placeholder-neutral-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="your@email.com"
                required
              />
            </div>

            {/* Card Details */}
            <div className="bg-neutral-50 dark:bg-neutral-700/50 rounded-xl p-6 space-y-4 border-2 border-dashed border-neutral-300 dark:border-neutral-600">
              <p className="text-sm font-medium text-neutral-600 dark:text-neutral-400 mb-4">
                💳 Test Card (Demo Mode)
              </p>

              {/* Card Number */}
              <div>
                <label className="block text-sm font-semibold text-neutral-900 dark:text-white mb-2">
                  Card Number
                </label>
                <input
                  type="text"
                  placeholder="1234 5678 9012 3456"
                  value={cardNumber}
                  onChange={handleCardNumberChange}
                  maxLength={19}
                  className="w-full px-4 py-3 rounded-lg border border-neutral-200 dark:border-neutral-600 bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white placeholder-neutral-400 dark:placeholder-neutral-500 focus:outline-none focus:ring-2 focus:ring-blue-500 font-mono"
                  required
                />
                <p className="text-xs text-neutral-500 dark:text-neutral-400 mt-1">
                  Test: 4242 4242 4242 4242
                </p>
              </div>

              {/* Expiry & CVC */}
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-semibold text-neutral-900 dark:text-white mb-2">
                    Expiry
                  </label>
                  <input
                    type="text"
                    placeholder="MM/YY"
                    value={cardExpiry}
                    onChange={handleExpiryChange}
                    maxLength={5}
                    className="w-full px-4 py-3 rounded-lg border border-neutral-200 dark:border-neutral-600 bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white placeholder-neutral-400 dark:placeholder-neutral-500 focus:outline-none focus:ring-2 focus:ring-blue-500 font-mono"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-semibold text-neutral-900 dark:text-white mb-2">
                    CVC
                  </label>
                  <input
                    type="text"
                    placeholder="123"
                    value={cardCvc}
                    onChange={(e) => setCardCvc(e.target.value.replace(/\D/g, "").slice(0, 4))}
                    maxLength={4}
                    className="w-full px-4 py-3 rounded-lg border border-neutral-200 dark:border-neutral-600 bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white placeholder-neutral-400 dark:placeholder-neutral-500 focus:outline-none focus:ring-2 focus:ring-blue-500 font-mono"
                    required
                  />
                </div>
              </div>
            </div>

            {/* Submit Button */}
            <button
              type="submit"
              disabled={loading}
              className="w-full bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 disabled:from-neutral-400 disabled:to-neutral-400 text-white font-bold py-3 px-6 rounded-lg transition-all shadow-lg hover:shadow-xl disabled:cursor-not-allowed"
            >
              {loading ? (
                <span className="flex items-center justify-center">
                  <Loading size="sm" className="border-white mr-2" />
                  Processing...
                </span>
              ) : (
                `Donate $${typeof finalAmount === 'number' ? finalAmount.toFixed(2) : '0.00'}`
              )}
            </button>

            {/* Info */}
            <p className="text-xs text-neutral-500 dark:text-neutral-400 text-center">
              This is a demonstration. No real charges will be made.
            </p>
          </form>
        </div>

        {/* Why Support */}
        <div className="mt-12 grid md:grid-cols-3 gap-6">
          {[
            { icon: "🔧", title: "Better Tools", desc: "Improve vehicle management features" },
            { icon: "🌍", title: "Global Access", desc: "Make tools available worldwide" },
            { icon: "💡", title: "Innovation", desc: "Fund new and exciting features" },
          ].map((item, i) => (
            <div
              key={i}
              className="bg-white dark:bg-neutral-800 rounded-lg p-6 text-center shadow-md"
            >
              <div className="text-4xl mb-3">{item.icon}</div>
              <h3 className="font-bold text-neutral-900 dark:text-white mb-2">{item.title}</h3>
              <p className="text-sm text-neutral-600 dark:text-neutral-400">{item.desc}</p>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
