import React from "react";
import ReactDOM from "react-dom/client";
import { AuthProvider } from "react-oidc-context";
import AppRouter from "./AppRouter";
import { oidcConfig } from "./config/oidcConfig";
import "./index.css";

const onSigninCallback = (): void => {
  window.history.replaceState({}, document.title, window.location.pathname);
};

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <AuthProvider {...oidcConfig} onSigninCallback={onSigninCallback}>
      <AppRouter />
    </AuthProvider>
  </React.StrictMode>
);