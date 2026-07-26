import type { Auth0ClientOptions } from '@auth0/auth0-spa-js';
import { z } from 'zod';

/**
 * Defaults are the public dev-tenant values documented in `.env.example`; the
 * Docker image builds without a `.env`, so they have to remain reachable.
 */
const EnvSchema = z.object({
  VITE_API_URL: z.url().default('http://localhost:7000'),
  VITE_AUTH0_DOMAIN: z.string().min(1).default('dev-m3ve8sir.us.auth0.com'),
  VITE_AUTH0_CLIENT_ID: z
    .string()
    .min(1)
    .default('O7WVznRNsyNwwmX0taD3veM6Yj3hEjhc'),
  VITE_AUTH0_AUDIENCE: z.string().min(1).default('https://issue-tracker'),
  VITE_AUTH0_SCOPE: z
    .string()
    .min(1)
    .default('openid profile Issue Tracker email')
});

export type Env = z.infer<typeof EnvSchema>;

export function readEnv(source: unknown): Env {
  const result = EnvSchema.safeParse(source);
  if (!result.success) {
    throw new Error(
      `Invalid environment configuration:\n${z.prettifyError(result.error)}`
    );
  }
  return result.data;
}

const env = readEnv(import.meta.env);

export const serverUrl = env.VITE_API_URL;

export const baseUrl = `${serverUrl}/api`;

export const authSettings: Auth0ClientOptions = {
  domain: env.VITE_AUTH0_DOMAIN,
  client_id: env.VITE_AUTH0_CLIENT_ID,
  redirect_uri: `${window.location.origin}/signin-callback`,
  scope: env.VITE_AUTH0_SCOPE,
  audience: env.VITE_AUTH0_AUDIENCE,
  // Keeps the session after the post-login page reload (default is in-memory).
  cacheLocation: 'localstorage'
};
