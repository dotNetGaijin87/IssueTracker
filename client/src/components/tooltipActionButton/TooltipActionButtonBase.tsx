import type { ReactNode } from 'react';
import { Box, IconButton, Tooltip, type TooltipProps } from '@mui/material';

interface Props {
  title: string;
  icon: ReactNode;
  tooltipProps?: Partial<Omit<TooltipProps, 'title' | 'children'>>;
  onClick?: () => void;
}

function TooltipActionButtonBase({
  title,
  icon,
  tooltipProps,
  onClick
}: Props) {
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
      <Tooltip title={title} {...tooltipProps}>
        <IconButton onClick={onClick}>{icon}</IconButton>
      </Tooltip>
    </Box>
  );
}

export default TooltipActionButtonBase;
