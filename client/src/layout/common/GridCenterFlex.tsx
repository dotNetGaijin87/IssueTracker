import { Grid } from '@mui/material';

interface Props {
  children: JSX.Element;
}

function GridCenterFlex({ children }: Props) {
  return (
    <Grid
      container
      spacing={2}
      direction="column"
      alignItems="center"
      alignContent="center"
      justifyContent="center"
    >
      <Grid
        container
        direction="column"
        alignItems="center"
        alignContent="center"
        justifyContent="center"
        style={{
          maxWidth: '90%',
          marginTop: '50px'
        }}
      >
        {children}
      </Grid>
    </Grid>
  );
}

export default GridCenterFlex;
