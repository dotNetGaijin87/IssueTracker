import LoadingPage from './LoadingPage';
import { useAuth } from '../../authentication/Auth';

interface Props {
  children: JSX.Element;
}

function AuthorizedPage({ children }: Props) {
  const { isAuthenticated, authInProgress } = useAuth();

  if (authInProgress) return <LoadingPage />;

  if (isAuthenticated) {
    return <>{children}</>;
  } else {
    return <div title="You do not have access to this page">{null}</div>;
  }
}

export default AuthorizedPage;
