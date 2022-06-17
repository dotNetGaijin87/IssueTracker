import { Divider } from '@mui/material';

function VerticalDivider() {
  return (
    <Divider
      flexItem
      sx={{ ml: 3, mr: 3 }}
      orientation="vertical"
      variant="middle"
    />
  );
}

export default VerticalDivider;
