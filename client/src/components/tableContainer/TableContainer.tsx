import { tableContainerStyle } from './styles';
import { TableContainer as MuiTableContainer, Paper } from '@mui/material';

interface Props {
  children: React.ReactNode;
}

function TableContainer({ children }: Props) {
  return (
    <MuiTableContainer component={Paper} sx={tableContainerStyle}>
      {children}
    </MuiTableContainer>
  );
}

export default TableContainer;
