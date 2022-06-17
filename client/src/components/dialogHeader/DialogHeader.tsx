import { Typography } from '@mui/material';

interface Props {
  text: string;
}

function DialogHeader({ text }: Props) {
  return (
    <Typography variant="h6" sx={{ m: 2 }}>
      {text}
    </Typography>
  );
}

export default DialogHeader;
