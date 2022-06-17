import { toast } from 'react-toastify';

function displayError(exception: any, fallbackMessage: string) {
  if (exception?.message) {
    toast.error(exception?.message);
  } else {
    toast.error(fallbackMessage);
  }
}

export default displayError;
