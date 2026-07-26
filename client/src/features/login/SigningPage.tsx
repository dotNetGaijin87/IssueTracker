import { useEffect } from 'react';
import { useAuth } from '@/authentication/Auth';
import LoadingPage from '@/layout/common/LoadingPage';

function SigningInPage() {
  const { signIn } = useAuth();

  useEffect(() => {
    signIn();
  }, [signIn]);

  return <LoadingPage />;
}

export default SigningInPage;
