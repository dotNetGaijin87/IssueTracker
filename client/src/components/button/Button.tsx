import { Button as MuiButton } from '@mui/material';

interface Props {
  onClick: () => void;
  label: string;
}

function Button({ label, onClick }: Props) {
  return (
    <MuiButton variant="text" color="secondary" onClick={onClick}>
      {label}
    </MuiButton>
  );
}

export default Button;
