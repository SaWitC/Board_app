import { Environment } from "./abstractions/environment";

export const environment: Environment = {
  apiUrl: 'https://localhost:7212/api',
  auth: {
    clientId: 'vdYUsy3KdRTgIKi54mgSDQlYmGpSuJZl',
    domain: 'inno-board-prod.us.auth0.com',
    audience: 'inno-board-api-prod',
    access_token_uri: 'https://localhost:7212/*',
    isBypassAuthorization: false,
  },
}
