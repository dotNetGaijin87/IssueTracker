import { Grid } from '@mui/material';

interface Props {
  children: React.ReactNode;
}

function Panel({ children }: Props) {
  return (
    <Grid
      sx={{
        bgcolor: 'background.paper',
        borderRadius: '14px',
        padding: 2,
        height: 'inherit',
        border: '1px solid',
        borderColor: 'divider',
        overflowY: 'hidden'
      }}
    >
      {children}
    </Grid>
  );
}

export default Panel;
