import { useEffect } from 'react';
import { useAuth } from '@/authentication/Auth';
import LoadingPage from '@/layout/common/LoadingPage';

function SigningInPage() {
  const { signIn } = useAuth();

  useEffect(() => {
    signIn();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return <LoadingPage />;
}

export default SigningInPage;
