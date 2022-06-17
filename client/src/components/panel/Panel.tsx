import { Grid } from '@mui/material';

interface Props {
  children: React.ReactNode;
}

function Panel({ children }: Props) {
  return (
    <Grid
      sx={{
        bgcolor: 'background.paper',
        borderRadius: 2,
        padding: 1,
        height: 'inherit',
        borderColor: 'divider',
        borderWidth: '1px',
        borderStyle: 'solid',
        overflowY: 'hidden'
      }}
    >
      {children}
    </Grid>
  );
}

export default Panel;
