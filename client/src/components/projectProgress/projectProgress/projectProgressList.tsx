import { Box, Typography } from '@mui/material';
import { ProjectProgress } from '../../../models/project/projectProgress';
import BaseBadge from '../../baseBadge/BaseBadge';

const projectProgressList = (borderless: boolean | undefined) => [
  {
    element: (
      <Box component="span">
        <Typography>{'N/A'}</Typography>
      </Box>
    ),
    value: ProjectProgress.Unspecified
  },
  {
    element: (
      <BaseBadge
        value={ProjectProgress.Open}
        style={{
          color: '#b75709',
          backgroundColor: '#b7570913'
        }}
        unstyled={borderless}
      />
    ),
    value: ProjectProgress.Open
  },
  {
    element: (
      <BaseBadge
        value={ProjectProgress.Closed}
        style={{
          color: '#0e9a31',
          backgroundColor: '#0e9a3116'
        }}
        unstyled={borderless}
      />
    ),
    value: ProjectProgress.Closed
  }
];

export default projectProgressList;
