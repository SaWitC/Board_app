import { Environment } from "./abstractions/environment";

export const environment: Environment = {
  apiUrl: 'https://localhost:7000/api',
  auth: {
    clientId: 'MpkBlYkrIbSbNlZaQneqWMPE5b0lGW6i',
    domain: 'dev-706vy0nkm3vbfojx.us.auth0.com',
    audience: 'inno-board-api-prod',
    access_token_uri: 'https://localhost:7212/*',
    isBypassAuthorization: false,
  },
}
