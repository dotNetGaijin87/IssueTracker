import type { ChangeEvent } from 'react';
import { Box, Pagination as MuiPagination } from '@mui/material';

interface Props {
  pageCount: number;
  page: number;
  onChange: (event: ChangeEvent<unknown>, page: number) => void;
}

function Pagination({ pageCount, page, onChange }: Props) {
  return (
    <Box
      display="flex"
      justifyContent="center"
      sx={{ backgroundColor: 'background.default', mt: 1, p: 1 }}
    >
      <MuiPagination
        count={pageCount}
        page={page}
        onChange={onChange}
        shape="rounded"
        showFirstButton
        showLastButton
      />
    </Box>
  );
}

export default Pagination;
