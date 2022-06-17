export const tableContainerStyle = {
  '&.MuiTableContainer-root': {
    bgcolor: 'background.paper',
    boxShadow: 'none',
    backgroundImage: 'none',
    borderRadius: 2,
    maxHeight: 600,
    mt: 1,
    minWidth: 200,
    borderColor: 'divider',
    borderWidth: '1px',
    borderStyle: 'solid',
    '.MuiTable-root': {
      overflowX: 'initial'
    },
    '.MuiTableCell-root': {
      padding: 0.5
    },
    table: {
      whiteSpace: 'nowrap',
      '& thead th': {
        borderBottomColor: 'divider',
        borderWidth: '1px',
        borderStyle: 'solid',

        zIndex: 600,
        fallbacks: {
          position: '-webkit-sticky'
        }
      },
      '.MuiTableRow-root td': {
        borderBottomColor: 'divider',
        borderWidth: '1px',
        borderStyle: 'solid'
      },

      '.MuiTableRow-root': {
        margin: 0,

        overflow: 'hidden'
      }
    }
  }
};
