import { Box, Typography } from '@mui/material';
import { IssueType } from '../../../models/issue/issueType';
import ShowChartIcon from '@mui/icons-material/ShowChart';
import PestControlIcon from '@mui/icons-material/PestControl';
import BaseBadge from '../../baseBadge/BaseBadge';

const issueTypeList = (unstyled: boolean | undefined) => [
  {
    element: (
      <Box component="span">
        <Typography>{'N/A'}</Typography>
      </Box>
    ),
    value: IssueType.Unspecified
  },
  {
    element: (
      <BaseBadge
        value={IssueType.Bug}
        style={{
          color: '#f24e34'
        }}
        unstyled={unstyled}
      />
    ),
    value: IssueType.Bug,
    icon: <PestControlIcon sx={{ color: '#ffffff20' }} />
  },
  {
    element: (
      <BaseBadge
        value={IssueType.Improvement}
        style={{
          color: '#7fb115'
        }}
        unstyled={unstyled}
      />
    ),
    value: IssueType.Improvement,
    icon: <ShowChartIcon sx={{ color: '#ffffff20' }} />
  }
];

export default issueTypeList;
