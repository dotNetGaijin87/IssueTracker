import * as React from 'react';
import { Box, IconButton, Tooltip } from '@mui/material';

interface Props {
  title: string;
  icon: React.ReactNode;
  tooltipArgs?: any;
  action?: () => Promise<void>;
}

function TooltipActionButtonBase({ title, icon, tooltipArgs, action }: Props) {
  return (
    <Box
      sx={{
        bgcolor: 'background.paper',
        m: 0.5,
        borderRadius: 1,
        borderColor: 'divider',
        borderWidth: '1px',
        borderStyle: 'solid'
      }}
    >
      <Tooltip title={title} {...tooltipArgs}>
        <IconButton onClick={action}>{icon}</IconButton>
      </Tooltip>
    </Box>
  );
}

export default TooltipActionButtonBase;
