import { LoadingButton as MuiLoadingButton } from '@mui/lab';

interface Props {
  label: string;
  loading: boolean;
  onClick: () => void;
}

function LoadingButton({ label, loading, onClick }: Props) {
  return (
    <MuiLoadingButton
      variant="text"
      color="secondary"
      loading={loading}
      onClick={onClick}
    >
      {label}
    </MuiLoadingButton>
  );
}

export default LoadingButton;
