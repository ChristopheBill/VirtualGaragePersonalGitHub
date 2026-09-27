import React from "react";
import { RouterProvider } from "react-router-dom";
import { useAuth } from "./providers/AuthProvider";
import router from "./router";
import { DarkModeProvider } from "./providers/DarkModeProvider";
import Loading from "./components/common/Loading";

function AuthInspector({ children }: { children: React.ReactNode }) {
  const auth = useAuth();

  if (auth.isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen bg-white dark:bg-neutral-900">
        <div className="text-center">
          <Loading />
          <p className="text-neutral-600 dark:text-neutral-400">Initializing...</p>
        </div>
      </div>
    );
  }

  return <>{children}</>;
}

export default function AppRouter() {
  return (
    <DarkModeProvider>
      <AuthInspector>
        <RouterProvider router={router} />
      </AuthInspector>
    </DarkModeProvider>
  );
}
