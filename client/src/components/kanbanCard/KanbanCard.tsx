import { Grid } from '@mui/material';

interface Props {
  children: JSX.Element;
}

function KanbanCard({ children }: Props) {
  return (
    <Grid
      item
      sx={{
        bgcolor: 'background.paper',
        borderRadius: 1,
        padding: 0,
        margin: 1,
        width: 170,
        minHeight: 200,
        borderColor: 'divider',
        borderWidth: '2px',
        borderStyle: 'solid'
      }}
    >
      {children}
    </Grid>
  );
}

export default KanbanCard;
