import { useEffect } from 'react';
import { useAuth } from '@/authentication/Auth';
import LoadingPage from '@/layout/common/LoadingPage';

function SigningOutPage() {
  const { signOut } = useAuth();

  useEffect(() => {
    signOut();
  }, [signOut]);

  return <LoadingPage />;
}

export default SigningOutPage;
