import createAuth0Client, { type Auth0Client } from '@auth0/auth0-spa-js';
import { authSettings } from '@/AppSettings';

let clientPromise: Promise<Auth0Client> | undefined;

/** One client per page load — `createAuth0Client` is expensive and stateful. */
export function getAuth0Client(): Promise<Auth0Client> {
  clientPromise ??= createAuth0Client(authSettings);
  return clientPromise;
}

export async function getAccessToken(): Promise<string | undefined> {
  try {
    const client = await getAuth0Client();
    return await client.getTokenSilently();
  } catch {
    return undefined;
  }
}
