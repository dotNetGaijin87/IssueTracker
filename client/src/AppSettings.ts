export const serverUrl = 'http://localhost:7000';

export const baseUrl = `${serverUrl}/api`;

export const authSettings = {
  domain: 'dev-m3ve8sir.us.auth0.com',
  client_id: 'O7WVznRNsyNwwmX0taD3veM6Yj3hEjhc',
  redirect_uri: window.location.origin + '/signin-callback',
  scope: 'openid profile Issue Tracker email',
  audience: 'https://issue-tracker'
};
