import { Box, Typography } from '@mui/material';
import { IssueType } from '../../../models/issue/issueType';

interface Props {
  value: IssueType;
  unstyled?: boolean;
  style: any;
}

function IssueTypeBaseBadge({ value, unstyled, style }: Props) {
  return unstyled ? (
    <Box component="span">
      <Typography>{value}</Typography>
    </Box>
  ) : (
    <div style={{ display: 'flex', justifyContent: 'center' }}>
      <Box
        sx={{
          boxSizing: 'border-box',
          width: '120px',
          height: '30px',
          borderRadius: 4,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          ...style
        }}
      >
        <Typography>{value}</Typography>
      </Box>
    </div>
  );
}

export default IssueTypeBaseBadge;
