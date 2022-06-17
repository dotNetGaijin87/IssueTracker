import { useAuth } from '../../authentication/Auth';
import LoadingPage from '../../layout/common/LoadingPage';

function SigningInPage() {
  const { signIn } = useAuth();
  signIn();
  return <LoadingPage />;
}

export default SigningInPage;
