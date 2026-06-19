const env = import.meta.env;

export const serverUrl = env.VITE_API_URL ?? 'http://localhost:7000';

export const baseUrl = `${serverUrl}/api`;

export const authSettings = {
  domain: env.VITE_AUTH0_DOMAIN ?? 'dev-m3ve8sir.us.auth0.com',
  client_id: env.VITE_AUTH0_CLIENT_ID ?? 'O7WVznRNsyNwwmX0taD3veM6Yj3hEjhc',
  redirect_uri: window.location.origin + '/signin-callback',
  scope: env.VITE_AUTH0_SCOPE ?? 'openid profile Issue Tracker email',
  audience: env.VITE_AUTH0_AUDIENCE ?? 'https://issue-tracker',
  // Keeps the session after the post-login page reload (default is in-memory).
  cacheLocation: 'localstorage' as const
};
