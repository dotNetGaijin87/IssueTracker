import { Backdrop, Box, Typography } from '@mui/material';

function SigningedOutPage() {
  return (
    <Backdrop open={true} invisible={true}>
      <Box
        sx={{
          width: '100%',
          justifyContent: 'center',
          position: 'fixed',
          top: '30%'
        }}
      >
        <Typography
          color="text.icon"
          sx={{
            textAlign: 'center'
          }}
          variant="h5"
        >
          Signed out
        </Typography>
      </Box>
    </Backdrop>
  );
}

export default SigningedOutPage;
