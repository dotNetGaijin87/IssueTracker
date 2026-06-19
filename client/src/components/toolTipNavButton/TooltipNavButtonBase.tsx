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
    <Tooltip title={title} placement="right">
      <IconButton
        component={NavLink}
        to={routeTo}
        sx={{
          color: 'text.secondary',
          borderRadius: 2,
          my: 0.5,
          transition: 'color .15s ease, background-color .15s ease',
          '&:hover': {
            color: 'text.primary',
            bgcolor: 'action.hover'
          },
          '&.active': {
            color: 'primary.main',
            bgcolor: 'action.selected'
          }
        }}
      >
        {icon}
      </IconButton>
    </Tooltip>
  );
}

export default TooltipNavButtonBase;
