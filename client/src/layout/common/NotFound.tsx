import { Box, Typography } from '@mui/material';

function NotFound() {
  return (
    <Box sx={{ textAlign: 'center', mt: 10 }}>
      <Typography
        sx={{
          fontSize: '4rem',
          fontWeight: 800,
          color: 'primary.main',
          lineHeight: 1
        }}
      >
        404
      </Typography>
      <Typography variant="h6" sx={{ mt: 1, color: 'text.secondary' }}>
        Page not found
      </Typography>
    </Box>
  );
}

export default NotFound;
