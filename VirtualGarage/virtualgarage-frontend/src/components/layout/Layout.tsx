import UserHeader from "./UserHeader";
import { Outlet } from "react-router-dom";

type LayoutProps = {
  children?: React.ReactNode;
};

export default function Layout({ children }: LayoutProps) {
  return (
    <div className="min-h-screen bg-neutral-50 dark:bg-neutral-900 text-neutral-900 dark:text-white transition-colors">
      <UserHeader />

      <main className="px-6 py-10">
        {children ?? <Outlet />}
      </main>
    </div>
  );
}
