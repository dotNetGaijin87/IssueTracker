import { Backdrop, CircularProgress, Stack } from '@mui/material';

function LoadingPage() {
  return (
    <Backdrop open={true} invisible={true}>
      <Stack sx={{ color: 'grey.500' }} spacing={2} direction="row">
        <CircularProgress color="secondary" />
      </Stack>
    </Backdrop>
  );
}

export default LoadingPage;
