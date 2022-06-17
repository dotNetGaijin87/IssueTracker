import * as React from 'react';
import { NavLink } from 'react-router-dom';
import { IconButton, Tooltip } from '@mui/material';

interface Props {
  title: string;
  routeTo: string;
  icon: React.ReactNode;
}

function TooltipNavButtonBase({ title, routeTo, icon }: Props) {
  return (
    <Tooltip
      title={title}
      sx={{
        color: 'text.icon',
        '&:hover': {
          bgcolor: 'transparent'
        },
        '&.active': {
          color: 'secondary.main'
        }
      }}
      placement="right"
    >
      <IconButton component={NavLink} to={routeTo}>
        {icon}
      </IconButton>
    </Tooltip>
  );
}

export default TooltipNavButtonBase;
