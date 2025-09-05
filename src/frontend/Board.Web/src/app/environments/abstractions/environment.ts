export interface Environment {
  apiUrl: string;
  auth: {
    clientId: string;
    authority: string;
    redirectUri: string;
    postLogoutRedirectUri: string;
    navigateToLoginRequestUrl: boolean;
  };
}
