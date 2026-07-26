/* eslint-disable react-refresh/only-export-components */
import React from 'react';
import { adapter } from '@/adapters/adapter';
import { authSettings } from '@/AppSettings';
import extractUserRole from '@/helpers/auth/extractUserRole';
import { UserIdSchema, type UserId } from '@/models/ids';
import type { UserRole } from '@/models/user/userRole';
import { getAuth0Client } from './authClient';

export type AuthUser = {
  /** The API's user id: Auth0's `name` claim is what the backend persists. */
  id: UserId | undefined;
  name: string | undefined;
  email: string | undefined;
  role: UserRole | undefined;
};

type Auth0ContextValue = {
  authUser: AuthUser | undefined;
  isAuthenticated: boolean;
  authInProgress: boolean;
  signIn: () => void;
  signOut: () => void;
};

/** Placeholder for consumers rendered outside an `AuthProvider`. */
const notReady = () => undefined;

export const Auth0Context = React.createContext<Auth0ContextValue>({
  authUser: undefined,
  authInProgress: true,
  isAuthenticated: false,
  signIn: notReady,
  signOut: notReady
});

export const useAuth = () => React.useContext(Auth0Context);

export const AuthProvider: React.FC<React.PropsWithChildren> = ({
  children
}) => {
  const [isAuthenticated, setIsAuthenticated] = React.useState(false);
  const [authUser, setAuthUser] = React.useState<AuthUser | undefined>(
    undefined
  );
  const [authInProgress, setAuthInProgress] = React.useState(true);

  React.useEffect(() => {
    let cancelled = false;

    const initialise = async () => {
      const client = await getAuth0Client();

      if (
        window.location.pathname === '/signin-callback' &&
        window.location.search.includes('code=')
      ) {
        await client.handleRedirectCallback();
        window.location.replace(`${window.location.origin}/kanban`);
        return;
      }

      if (!(await client.isAuthenticated())) return;

      const user = await client.getUser();
      const id = UserIdSchema.safeParse(user?.name);
      if (!id.success) return;

      await adapter.User.createSafely();
      if (cancelled) return;

      setAuthUser({
        id: id.data,
        name: user?.name,
        email: user?.email,
        role: extractUserRole(user)
      });
      setIsAuthenticated(true);
    };

    void initialise().finally(() => {
      if (!cancelled) setAuthInProgress(false);
    });

    return () => {
      cancelled = true;
    };
  }, []);

  const signIn = React.useCallback(() => {
    void getAuth0Client().then((client) => client.loginWithRedirect());
  }, []);

  const signOut = React.useCallback(() => {
    void getAuth0Client().then((client) =>
      client.logout({
        client_id: authSettings.client_id,
        returnTo: `${window.location.origin}/signout-callback`
      })
    );
  }, []);

  const value = React.useMemo<Auth0ContextValue>(
    () => ({ isAuthenticated, authUser, authInProgress, signIn, signOut }),
    [isAuthenticated, authUser, authInProgress, signIn, signOut]
  );

  return (
    <Auth0Context.Provider value={value}>{children}</Auth0Context.Provider>
  );
};
