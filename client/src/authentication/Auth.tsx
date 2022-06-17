import React from 'react';
import createAuth0Client, { Auth0Client } from '@auth0/auth0-spa-js';
import { authSettings } from '../AppSettings';
import { adapter } from '../adapters/adapter';
import { UserRole } from '../models/user/userRole';
import extractUserRole from '../helpers/auth/extractUserRole';
import extractUserId from '../helpers/auth/extractUserId';

interface Auth0User {
  name?: string;
  email?: string;
  sub?: string;
  role?: UserRole;
}
interface IAuth0Context {
  authUser?: Auth0User;
  isAuthenticated: boolean;
  authInProgress: boolean;
  signIn: () => void;
  signOut: () => void;
}
export const Auth0Context = React.createContext<IAuth0Context>({
  authInProgress: true,
  isAuthenticated: false,
  signIn: () => {},
  signOut: () => {}
});

export const useAuth = () => React.useContext(Auth0Context);

export const AuthProvider: React.FC = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = React.useState<boolean>(false);
  const [authUser, setAuthUser] = React.useState<Auth0User | undefined>(
    undefined
  );
  const [auth0Client, setAuth0Client] = React.useState<Auth0Client>();
  const [authInProgress, setLoading] = React.useState<boolean>(true);

  React.useEffect(() => {
    const initAuth0 = async () => {
      setLoading(true);
      const auth0FromHook = await createAuth0Client(authSettings);
      setAuth0Client(auth0FromHook);

      if (
        window.location.pathname === '/signin-callback' &&
        window.location.search.indexOf('code=') > -1
      ) {
        await auth0FromHook.handleRedirectCallback();
        window.location.replace(window.location.origin + '/kanban');
      }

      const isAuthenticatedFromHook = await auth0FromHook.isAuthenticated();
      if (isAuthenticatedFromHook) {
        const user = await auth0FromHook.getUser();
        if (user?.sub !== undefined) {
          await adapter.User.createSafely(extractUserId(user?.sub));
          setAuthUser({ ...user, role: extractUserRole(user) });
          setIsAuthenticated(isAuthenticatedFromHook);
        }
      }

      setLoading(false);
    };
    initAuth0();
  }, []);

  const getAuth0ClientFromState = () => {
    if (auth0Client === undefined) {
      throw new Error('Auth0 client is undefined');
    }
    return auth0Client;
  };

  return (
    <Auth0Context.Provider
      value={{
        isAuthenticated,
        authUser,
        signIn: () => {
          getAuth0ClientFromState()?.loginWithRedirect();
        },
        signOut: () =>
          getAuth0ClientFromState()?.logout({
            client_id: authSettings.client_id,
            returnTo: window.location.origin + '/signout-callback'
          }),
        authInProgress
      }}
    >
      {children}
    </Auth0Context.Provider>
  );
};

export const getAccessToken = async () => {
  const auth0FromHook = await createAuth0Client(authSettings);
  const accessToken = await auth0FromHook.getTokenSilently();
  return accessToken;
};
