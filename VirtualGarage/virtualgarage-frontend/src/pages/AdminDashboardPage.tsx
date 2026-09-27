import { useState, useEffect } from "react";
import { getAllDonations, type DonationRecord } from "../api/donations";
import UserList from "../components/users/UserList";
import Loading from "../components/common/Loading";

type TabType = "users" | "donations";

function formatAmount(amount: number, currency: string) {
  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency,
  }).format(amount / 100);
}

export default function AdminDashboardPage() {
  const [activeTab, setActiveTab] = useState<TabType>("users");
  const [donations, setDonations] = useState<DonationRecord[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (activeTab === "donations") {
      const load = async () => {
        setLoading(true);
        setError(null);
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
    }
  }, [activeTab]);

  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-3xl font-bold text-neutral-900 dark:text-white">
          Admin Dashboard
        </h1>
        <p className="text-neutral-600 dark:text-neutral-400 mt-2">
          Manage users and view donations
        </p>
      </div>

      {/* Tabs */}
      <div className="border-b border-neutral-200 dark:border-neutral-700">
        <nav className="flex gap-8">
          <button
            onClick={() => setActiveTab("users")}
            className={`py-3 px-1 border-b-2 font-semibold text-sm transition-colors ${
              activeTab === "users"
                ? "border-blue-600 text-blue-600 dark:border-blue-500 dark:text-blue-500"
                : "border-transparent text-neutral-600 dark:text-neutral-400 hover:text-neutral-900 dark:hover:text-neutral-200 hover:border-neutral-300 dark:hover:border-neutral-600"
            }`}
          >
            👥 Users
          </button>
          <button
            onClick={() => setActiveTab("donations")}
            className={`py-3 px-1 border-b-2 font-semibold text-sm transition-colors ${
              activeTab === "donations"
                ? "border-blue-600 text-blue-600 dark:border-blue-500 dark:text-blue-500"
                : "border-transparent text-neutral-600 dark:text-neutral-400 hover:text-neutral-900 dark:hover:text-neutral-200 hover:border-neutral-300 dark:hover:border-neutral-600"
            }`}
          >
            💰 Donations
          </button>
        </nav>
      </div>

      {/* Content */}
      <div>
        {activeTab === "users" && (
          <div className="flex justify-center">
            <div className="w-full max-w-4xl">
              <UserList />
            </div>
          </div>
        )}

        {activeTab === "donations" && (
          <>
            {loading && (
              <div className="flex items-center justify-center py-12">
                <div className="text-center">
                  <Loading />
                  <p className="text-neutral-600 dark:text-neutral-400">Loading donations...</p>
                </div>
              </div>
            )}

            {error && (
              <div className="bg-red-50 dark:bg-red-900/40 border border-red-200 dark:border-red-800 text-red-800 dark:text-red-100 p-4 rounded-lg">
                {error}
              </div>
            )}

            {!loading && !error && (
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
            )}
          </>
        )}
      </div>
    </div>
  );
}
