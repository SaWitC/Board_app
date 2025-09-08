export interface Environment {
  apiUrl: string;
  auth: {
    clientId: string;
    domain: string;
    audience: string;
    access_token_uri: string;
  };
}
