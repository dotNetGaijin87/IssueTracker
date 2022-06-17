import { Box, Typography } from '@mui/material';
import { IssuePermission } from '../../models/issue/issuePermission';
import EditIcon from '@mui/icons-material/Edit';
import VisibilityIcon from '@mui/icons-material/Visibility';

const permissionBadgeList = () => [
  {
    element: (
      <Box component="span">
        <Typography>{'N/A'}</Typography>
      </Box>
    )
  },
  {
    element: (
      <Box
        sx={{
          boxSizing: 'border-box',
          mt: 1,
          mb: 1,
          height: '30px',
          borderRadius: 4,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          color: '#ca6822'
        }}
      >
        <Typography sx={{ m: 1 }}>Editing...</Typography>
      </Box>
    ),
    value: IssuePermission.CanDelete
  },
  {
    element: (
      <Box
        sx={{
          boxSizing: 'border-box',
          mt: 1,
          mb: 1,
          height: '30px',
          borderRadius: 4,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          bgcolor: '#ffffff10',
          color: '#79797984'
        }}
      >
        <EditIcon sx={{ m: 0.5 }} />
        <Typography sx={{ m: 1 }}>You can modify this page</Typography>
      </Box>
    ),
    value: IssuePermission.CanModify
  },
  {
    element: (
      <Box
        sx={{
          boxSizing: 'border-box',
          mt: 1,
          mb: 1,
          height: '30px',
          borderRadius: 4,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          bgcolor: '#ffffff10',
          color: '#79797984'
        }}
      >
        <VisibilityIcon sx={{ m: 0.5 }} />
        <Typography sx={{ m: 1 }}>You can only see this page</Typography>
      </Box>
    ),
    value: IssuePermission.CanSee
  }
];

export default permissionBadgeList;
