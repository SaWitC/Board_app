import { Environment } from "./abstractions/environment";

export const environment: Environment = {
  apiUrl: 'https://localhost:7212/api',
  auth: {
    clientId: '1234567890',
    authority: 'https://login.microsoftonline.com/1234567890',
    redirectUri: 'https://localhost:4200',
    postLogoutRedirectUri: 'https://localhost:4200',
    navigateToLoginRequestUrl: false
  }
}
