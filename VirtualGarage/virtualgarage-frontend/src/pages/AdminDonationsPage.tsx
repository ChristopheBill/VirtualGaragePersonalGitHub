import { useEffect, useState } from "react";
import { getAllDonations } from "../api/donations";
import type { DonationRecord } from "../api/donations";

function formatAmount(amount: number, currency: string) {
  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency,
  }).format(amount / 100);
}

export default function AdminDonationsPage() {
  const [donations, setDonations] = useState<DonationRecord[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const load = async () => {
      try {
        const data = await getAllDonations();
        setDonations(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to load donations");
      } finally {
        setLoading(false);
      }
    };
    load();
  }, []);

  if (loading) {
    return (
      <div className="flex items-center justify-center py-12">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 dark:border-blue-500 mx-auto mb-4" />
          <p className="text-neutral-600 dark:text-neutral-400">Loading donations...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="bg-red-50 dark:bg-red-900/40 border border-red-200 dark:border-red-800 text-red-800 dark:text-red-100 p-4 rounded-lg">
        {error}
      </div>
    );
  }

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-3xl font-bold text-neutral-900 dark:text-white">Donations</h1>
        <p className="text-neutral-600 dark:text-neutral-400 mt-2">
          Admin view of all donations
        </p>
      </div>

      <div className="overflow-hidden rounded-xl border border-neutral-200 dark:border-neutral-700 shadow-sm">
        <table className="min-w-full divide-y divide-neutral-200 dark:divide-neutral-700">
          <thead className="bg-neutral-50 dark:bg-neutral-800">
            <tr>
              <th className="px-4 py-3 text-left text-xs font-semibold uppercase tracking-wide text-neutral-600 dark:text-neutral-300">Date</th>
              <th className="px-4 py-3 text-left text-xs font-semibold uppercase tracking-wide text-neutral-600 dark:text-neutral-300">User</th>
              <th className="px-4 py-3 text-left text-xs font-semibold uppercase tracking-wide text-neutral-600 dark:text-neutral-300">Email</th>
              <th className="px-4 py-3 text-left text-xs font-semibold uppercase tracking-wide text-neutral-600 dark:text-neutral-300">Amount</th>
              <th className="px-4 py-3 text-left text-xs font-semibold uppercase tracking-wide text-neutral-600 dark:text-neutral-300">Status</th>
              <th className="px-4 py-3 text-left text-xs font-semibold uppercase tracking-wide text-neutral-600 dark:text-neutral-300">Payment Intent</th>
            </tr>
          </thead>
          <tbody className="bg-white dark:bg-neutral-900 divide-y divide-neutral-200 dark:divide-neutral-800">
            {donations.map((d) => (
              <tr key={d.id} className="hover:bg-neutral-50 dark:hover:bg-neutral-800/60">
                <td className="px-4 py-3 text-sm text-neutral-900 dark:text-neutral-100">
                  {new Date(d.createdAt).toLocaleString()}
                </td>
                <td className="px-4 py-3 text-sm text-neutral-700 dark:text-neutral-300 font-mono">
                  {d.userId}
                </td>
                <td className="px-4 py-3 text-sm text-neutral-700 dark:text-neutral-300">
                  {d.email}
                </td>
                <td className="px-4 py-3 text-sm font-semibold text-neutral-900 dark:text-neutral-100">
                  {formatAmount(d.amount, d.currency)}
                </td>
                <td className="px-4 py-3 text-sm">
                  <span className={`px-2 py-1 rounded-full text-xs font-semibold ${
                    d.status === "succeeded"
                      ? "bg-green-100 text-green-800 dark:bg-green-900/40 dark:text-green-200"
                      : "bg-yellow-100 text-yellow-800 dark:bg-yellow-900/40 dark:text-yellow-200"
                  }`}>
                    {d.status}
                  </span>
                </td>
                <td className="px-4 py-3 text-sm text-neutral-500 dark:text-neutral-400 font-mono">
                  {d.paymentIntentId}
                </td>
              </tr>
            ))}
            {donations.length === 0 && (
              <tr>
                <td colSpan={6} className="px-4 py-6 text-center text-neutral-500 dark:text-neutral-400">
                  No donations yet.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
