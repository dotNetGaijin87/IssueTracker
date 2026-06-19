import { Box, Typography } from '@mui/material';

interface Props {
  title: string;
  children: React.ReactNode;
}

function Bar({ title, children }: Props) {
  return (
    <Box
      display="flex"
      alignItems="center"
      sx={{
        bgcolor: 'background.paper',
        borderRadius: 2,
        border: '1px solid',
        borderColor: 'divider',
        px: 1.5
      }}
    >
      <Typography
        sx={{
          mr: 1,
          color: 'text.secondary',
          fontSize: '0.72rem',
          fontWeight: 700,
          textTransform: 'uppercase',
          letterSpacing: '0.06em',
          whiteSpace: 'nowrap'
        }}
      >
        {title}
      </Typography>
      <Box
        sx={{
          display: 'flex',
          alignItems: 'center',
          flexWrap: 'wrap'
        }}
      >
        {children}
      </Box>
    </Box>
  );
}

export default Bar;
