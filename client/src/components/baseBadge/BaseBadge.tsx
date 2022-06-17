import { Box, Typography } from '@mui/material';

interface Props {
  value: any;
  unstyled?: boolean;
  borderless?: boolean;
  style: any;
}

function BaseBadge({ value, unstyled, borderless, style }: Props) {
  return unstyled ? (
    <Box component="span">
      <Typography>{value}</Typography>
    </Box>
  ) : borderless ? (
    <Box component="span">
      <Typography sx={{ ...style, bgcolor: 'transparent' }}>{value}</Typography>
    </Box>
  ) : (
    <Box display="flex" justifyContent="center">
      <Box
        sx={{
          boxSizing: 'border-box',
          minWidth: '120px',
          height: '30px',
          borderRadius: 4,
          margin: 1,
          p: 1,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          ...style
        }}
      >
        <Typography>{value}</Typography>
      </Box>
    </Box>
  );
}

export default BaseBadge;
