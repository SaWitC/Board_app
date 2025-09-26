import { Environment } from "./abstractions/environment";

export const environment: Environment = {
  apiUrl: 'https://api.inno-board.inno.ws/api',
  auth: {
    clientId: 'MpkBlYkrIbSbNlZaQneqWMPE5b0lGW6i',
    domain: 'dev-706vy0nkm3vbfojx.us.auth0.com',
    audience: 'inno-board-api-prod',
    access_token_uri: 'https://inno-board.inno.ws/*',
    isBypassAuthorization: false,
  },
}
