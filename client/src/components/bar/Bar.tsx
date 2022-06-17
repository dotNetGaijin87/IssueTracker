import { Box, Typography } from '@mui/material';

interface Props {
  title: string;
  children: React.ReactNode;
}

function Bar({ title, children }: Props) {
  return (
    <Box
      display="flex"
      sx={{
        bgcolor: 'background.paper',
        borderRadius: 1,
        borderColor: 'divider',
        borderWidth: '1px',
        borderStyle: 'solid'
      }}
    >
      <Box component="span" sx={{ m: 1 }}>
        <Typography>{title}</Typography>
      </Box>
      <Box
        sx={{
          ml: 1,
          mr: 1,
          display: 'flex',
          justifyContent: 'space-between',
          bgcolor: 'background.paper',
          borderRadius: 1
        }}
      >
        {children}
      </Box>
    </Box>
  );
}

export default Bar;
