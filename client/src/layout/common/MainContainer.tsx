import { Grid } from '@mui/material';

interface Props {
  children: JSX.Element;
}

function MainContainer({ children }: Props) {
  return (
    <Grid
      container
      spacing={1}
      flexDirection="column"
      sx={{
        marginTop: `30px`,
        bgcolor: 'transparent',
        borderColor: 'transparent',
        borderStyle: 'solid',
        borderRadius: 2,
        height: '100%'
      }}
    >
      {children}
    </Grid>
  );
}

export default MainContainer;
