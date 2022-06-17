import { FormControl } from '@mui/material';

interface Props {
  children: React.ReactNode;
}

function Field({ children }: Props) {
  return (
    <FormControl sx={{ width: '200px', boxSizing: 'border-box' }}>
      {children}
    </FormControl>
  );
}

export default Field;
