import { describe, expect, it } from 'vitest';
import { readEnv } from './AppSettings';

describe('readEnv', () => {
  it('uses the documented dev defaults when nothing is provided', () => {
    const env = readEnv({});
    expect(env.VITE_API_URL).toBe('http://localhost:7000');
    expect(env.VITE_AUTH0_DOMAIN).toBe('dev-m3ve8sir.us.auth0.com');
  });

  it('prefers supplied values', () => {
    const env = readEnv({
      VITE_API_URL: 'https://api.example.com',
      VITE_AUTH0_DOMAIN: 'example.eu.auth0.com'
    });
    expect(env.VITE_API_URL).toBe('https://api.example.com');
    expect(env.VITE_AUTH0_DOMAIN).toBe('example.eu.auth0.com');
  });

  it('fails loudly on a malformed API url instead of silently booting', () => {
    expect(() => readEnv({ VITE_API_URL: 'not-a-url' })).toThrow(
      /Invalid environment configuration/
    );
  });

  it('rejects a blank Auth0 domain', () => {
    expect(() => readEnv({ VITE_AUTH0_DOMAIN: '' })).toThrow(
      /Invalid environment configuration/
    );
  });
});
