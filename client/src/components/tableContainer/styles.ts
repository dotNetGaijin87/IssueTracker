export const tableContainerStyle = {
  '&.MuiTableContainer-root': {
    bgcolor: 'background.paper',
    boxShadow: 'none',
    backgroundImage: 'none',
    borderRadius: '12px',
    maxHeight: 600,
    mt: 2,
    minWidth: 200,
    border: '1px solid',
    borderColor: 'divider',
    overflow: 'auto',
    '.MuiTable-root': {
      overflowX: 'initial'
    },
    '.MuiTableCell-root': {
      padding: '12px 16px',
      textAlign: 'left'
    },
    table: {
      whiteSpace: 'nowrap',
      '& thead th': {
        position: 'sticky',
        top: 0,
        zIndex: 600,
        backgroundColor: 'background.paper',
        color: 'text.secondary',
        fontSize: '0.72rem',
        fontWeight: 700,
        textTransform: 'uppercase',
        letterSpacing: '0.06em',
        borderBottom: '1px solid',
        borderColor: 'divider'
      },
      '& tbody .MuiTableRow-root': {
        transition: 'background-color .14s ease'
      },
      '& tbody .MuiTableRow-root:hover': {
        backgroundColor: 'action.hover'
      },
      '& tbody .MuiTableRow-root td': {
        borderBottom: '1px solid',
        borderColor: 'divider'
      },
      '& tbody .MuiTableRow-root:last-of-type td': {
        borderBottom: 'none'
      }
    }
  }
};
