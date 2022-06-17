import { Box, Typography } from '@mui/material';
import { IssuePriority } from '../../../models/issue/issuePriority';
import LooksOneIcon from '@mui/icons-material/LooksOne';
import LooksTwoIcon from '@mui/icons-material/LooksTwo';
import Looks3Icon from '@mui/icons-material/Looks3';
import Looks4Icon from '@mui/icons-material/Looks4';
import BaseBadge from '../../baseBadge/BaseBadge';

const issuePriorityList = (
  unstyled: boolean | undefined,
  borderless: boolean | undefined
) => [
  {
    element: (
      <Box component="span">
        <Typography>{'N/A'}</Typography>
      </Box>
    ),
    value: IssuePriority.Unspecified
  },
  {
    element: (
      <BaseBadge
        value={IssuePriority.Low}
        style={{
          color: '#08b587',
          backgroundColor: '#08b58712'
        }}
        unstyled={unstyled}
        borderless={borderless}
      />
    ),
    value: IssuePriority.Low,
    icon: <LooksOneIcon sx={{ color: '#7ee787' }} />
  },
  {
    element: (
      <BaseBadge
        value={IssuePriority.Medium}
        style={{
          color: '#1b9ec5',
          backgroundColor: '#1b9dc510'
        }}
        unstyled={unstyled}
        borderless={borderless}
      />
    ),
    value: IssuePriority.Medium,
    icon: <LooksTwoIcon sx={{ color: '#39b2d6' }} />
  },
  {
    element: (
      <BaseBadge
        value={IssuePriority.High}
        style={{
          color: '#ffa100',
          backgroundColor: '#ffa10029'
        }}
        unstyled={unstyled}
        borderless={borderless}
      />
    ),
    value: IssuePriority.High,
    icon: <Looks3Icon sx={{ color: '#cdc82a' }} />
  },
  {
    element: (
      <BaseBadge
        value={IssuePriority.Critical}
        style={{
          color: '#d14038',
          backgroundColor: '#d1403818'
        }}
        unstyled={unstyled}
        borderless={borderless}
      />
    ),
    value: IssuePriority.Critical,
    icon: <Looks4Icon sx={{ color: '#d6625b' }} />
  }
];

export default issuePriorityList;
