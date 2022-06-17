import { LoadingButton } from '@mui/lab';

interface Props {
  label: string;
  color?: 'secondary' | 'primary';
  loading: boolean;
  onClick: () => void;
}

function CommentButton({ label, loading, color = 'primary', onClick }: Props) {
  return (
    <LoadingButton
      size="small"
      color={color}
      sx={{ height: '20px', width: '80px' }}
      loading={loading}
      onClick={onClick}
    >
      {label}
    </LoadingButton>
  );
}

export default CommentButton;
