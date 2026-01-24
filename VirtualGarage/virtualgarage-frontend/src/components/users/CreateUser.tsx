import { useState } from "react";
import { createUser } from "../../api/users";

export default function CreateUser({ onCreated }: { onCreated: () => void }) {
  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    email: "",
  });

  const submit = async () => {
    await createUser(form);
    setForm({ firstName: "", lastName: "", email: "" });
    onCreated();
  };

  return (
    <div className="p-6 border border-neutral-200 dark:border-neutral-700 rounded-xl bg-white dark:bg-neutral-800 shadow-sm">
      <h2 className="font-semibold mb-4 text-base sm:text-lg text-neutral-900 dark:text-white">Create User</h2>

      <input
        className="border border-neutral-300 dark:border-neutral-600 p-3 w-full mb-3 rounded-lg text-base bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white placeholder-neutral-500 dark:placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
        placeholder="First name"
        value={form.firstName}
        onChange={(e) => setForm({ ...form, firstName: e.target.value })}
      />

      <input
        className="border border-neutral-300 dark:border-neutral-600 p-3 w-full mb-3 rounded-lg text-base bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white placeholder-neutral-500 dark:placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
        placeholder="Last name"
        value={form.lastName}
        onChange={(e) => setForm({ ...form, lastName: e.target.value })}
      />

      <input
        className="border border-neutral-300 dark:border-neutral-600 p-3 w-full mb-4 rounded-lg text-base bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white placeholder-neutral-500 dark:placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
        placeholder="Email"
        value={form.email}
        onChange={(e) => setForm({ ...form, email: e.target.value })}
      />

      <button
        onClick={submit}
        className="w-full bg-blue-600 text-white px-4 py-3 rounded-lg font-medium hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600 transition-colors focus:outline-none focus:ring-2 focus:ring-blue-500"
      >
        Create
      </button>
    </div>
  );
}
