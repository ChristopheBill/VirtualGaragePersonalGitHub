import { useState } from "react";
import CreateUser from "../components/users/CreateUser";
import UserList from "../components/users/UserList";

export default function UsersPage() {
  const [refresh, setRefresh] = useState(0);

  return (
    <div className="flex justify-center">
      <div className="w-full grid grid-cols-1 md:grid-cols-2 gap-4 md:gap-6 max-w-4xl">
        <CreateUser onCreated={() => setRefresh((r) => r + 1)} />
        <UserList key={refresh} />
      </div>
    </div>
  );
}
