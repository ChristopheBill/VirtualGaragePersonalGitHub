import { useEffect, useState } from "react";
import { getUsers } from "../../api/users";
import type { User } from "../../types/user";

export default function UserList() {
  const [users, setUsers] = useState<User[]>([]);

  useEffect(() => {
    getUsers().then(setUsers);
  }, []);

  return (
    <div className="p-6 border border-neutral-200 dark:border-neutral-700 rounded-xl bg-white dark:bg-neutral-800 shadow-sm">
      <h2 className="font-semibold mb-4 text-base sm:text-lg text-neutral-900 dark:text-white">Users</h2>

      <ul className="space-y-2">
        {users.map((u) => (
          <li key={u.id} className="border border-neutral-200 dark:border-neutral-700 p-3 rounded-lg text-sm sm:text-base text-neutral-900 dark:text-neutral-100 hover:bg-neutral-100 dark:hover:bg-neutral-700 transition-colors">
            {u.firstName} {u.lastName}
          </li>
        ))}
      </ul>
    </div>
  );
}
