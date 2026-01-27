import { useEffect, useState } from "react";
import { deleteUser, getUsers } from "../../api/users";
import type { User } from "../../types/user";

export default function UserList() {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const load = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getUsers();
      setUsers(data);
    } catch (err) {
      setError("Failed to load users");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
  }, []);

  const handleDelete = async (id: string) => {
    if (!confirm("Delete this user?")) return;
    try {
      await deleteUser(id);
      setUsers((prev) => prev.filter((u) => u.id !== id));
    } catch (err) {
      setError("Failed to delete user");
    }
  };

  return (
    <div className="p-6 border border-neutral-200 dark:border-neutral-700 rounded-xl bg-white dark:bg-neutral-800 shadow-sm">
      <h2 className="font-semibold mb-4 text-base sm:text-lg text-neutral-900 dark:text-white">Users</h2>

      {loading && <p className="text-sm text-neutral-500">Loading users…</p>}
      {error && <p className="text-sm text-red-500">{error}</p>}

      <ul className="space-y-2">
        {users.map((u) => (
          <li
            key={u.id}
            className="flex items-center justify-between border border-neutral-200 dark:border-neutral-700 p-3 rounded-lg text-sm sm:text-base text-neutral-900 dark:text-neutral-100 hover:bg-neutral-100 dark:hover:bg-neutral-700 transition-colors"
          >
            <span>
              {u.firstName} {u.lastName}
            </span>
            <button
              onClick={() => handleDelete(u.id)}
              className="text-red-600 hover:text-red-700 text-sm font-medium"
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}
