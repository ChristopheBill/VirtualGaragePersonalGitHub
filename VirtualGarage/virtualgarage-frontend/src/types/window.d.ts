declare global {
  interface Window {
    __auth_context__?: unknown;
  }
}

export {};