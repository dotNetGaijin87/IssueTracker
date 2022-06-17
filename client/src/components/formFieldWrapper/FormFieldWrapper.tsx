import { Box, Typography } from '@mui/material';

interface Props {
  title: string;
  highlighted?: boolean;
  children: React.ReactNode;
}

function FormFieldWrapper({ title, highlighted, children }: Props) {
  return (
    <Box display="flex" sx={{ width: '100%', boxSizing: 'border-box' }}>
      <Box component="span" sx={{ m: 1, minWidth: '30%' }}>
        <Typography
          sx={{
            overflowWrap: 'anywhere',
            color: highlighted ? 'secondary.main' : 'inherit'
          }}
        >
          {title}
        </Typography>
      </Box>
      {children}
    </Box>
  );
}

export default FormFieldWrapper;
