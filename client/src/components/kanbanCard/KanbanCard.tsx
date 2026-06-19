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
        borderRadius: 2,
        padding: 0,
        margin: 1,
        width: 170,
        minHeight: 200,
        borderColor: 'divider',
        borderWidth: '1px',
        borderStyle: 'solid',
        transition:
          'transform .16s ease, box-shadow .16s ease, border-color .16s ease',
        '&:hover': {
          transform: 'translateY(-2px)',
          borderColor: 'primary.main',
          boxShadow: '0 10px 28px rgba(0,0,0,0.35)'
        }
      }}
    >
      {children}
    </Grid>
  );
}

export default KanbanCard;
