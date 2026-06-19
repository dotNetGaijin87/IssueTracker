import { Box, Typography } from '@mui/material';

interface Props {
  text: string;
}

function DialogHeader({ text }: Props) {
  return (
    <Box
      sx={{
        px: 3,
        pt: 2.5,
        pb: 2,
        mb: 1,
        borderBottom: '1px solid',
        borderColor: 'divider'
      }}
    >
      <Typography variant="h6" sx={{ fontWeight: 600 }}>
        {text}
      </Typography>
    </Box>
  );
}

export default DialogHeader;
