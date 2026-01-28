import { AuthContextProps } from 'react-oidc-context';

declare global {
  interface Window {
    __auth_context__?: AuthContextProps;
  }
}

export {};