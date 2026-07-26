import { toast } from 'react-toastify';
import { errorMessage } from '@/adapters/apiError';

function displayError(error: unknown, fallbackMessage: string): void {
  toast.error(errorMessage(error, fallbackMessage));
}

export default displayError;
