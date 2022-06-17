import { Box, Typography } from '@mui/material';
import { IssueProgress } from '../../../models/issue/issueProgress';
import BaseBadge from '../../baseBadge/BaseBadge';

const issueProgressList = (unstyled: boolean | undefined) => [
  {
    element: (
      <Box component="span">
        <Typography>{'N/A'}</Typography>
      </Box>
    ),
    value: IssueProgress.Unspecified
  },
  {
    element: (
      <BaseBadge
        value={IssueProgress.ToDo}
        style={{
          bgcolor: '#50a7e61a',
          color: '#137ac3'
        }}
        unstyled={unstyled}
      />
    ),
    value: IssueProgress.ToDo
  },
  {
    element: (
      <BaseBadge
        value={IssueProgress.InProgress}
        style={{
          bgcolor: '#c9691540',
          color: '#c96915'
        }}
        unstyled={unstyled}
      />
    ),
    value: IssueProgress.InProgress
  },
  {
    element: (
      <BaseBadge
        value={IssueProgress.ToBeTested}
        style={{
          bgcolor: '#e622df30',
          color: '#c134bc'
        }}
        unstyled={unstyled}
      />
    ),
    value: IssueProgress.ToBeTested
  },
  {
    element: (
      <BaseBadge
        value={IssueProgress.Closed}
        style={{
          bgcolor: '#64ce2c0a',
          color: '#44a90d'
        }}
        unstyled={unstyled}
      />
    ),
    value: IssueProgress.Closed
  },
  {
    element: (
      <BaseBadge
        value={IssueProgress.OnHold}
        style={{
          bgcolor: '#9d79053d',
          color: '#9d7905'
        }}
        unstyled={unstyled}
      />
    ),
    value: IssueProgress.OnHold
  },
  {
    element: (
      <BaseBadge
        value={IssueProgress.Canceled}
        style={{
          bgcolor: '#f851490f',
          color: '#f85149'
        }}
        unstyled={unstyled}
      />
    ),
    value: IssueProgress.Canceled
  }
];

export default issueProgressList;
