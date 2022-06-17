import { useAuth } from '../../authentication/Auth';
import LoadingPage from '../../layout/common/LoadingPage';

function SigningOutPage() {
  const { signOut } = useAuth();
  signOut();

  return <LoadingPage />;
}

export default SigningOutPage;
