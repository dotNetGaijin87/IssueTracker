import { CircularProgress } from '@mui/material';
import React from 'react';
import TooltipActionButtonBase from './TooltipActionButtonBase';

interface Props {
  title: string;
  color?: string;
  icon: JSX.Element;
  args?: any;
  onClick: () => void;
}

function TooltipActionButton({ title, color, icon, args, onClick }: Props) {
  const [processing, setProcessing] = React.useState(false);

  return (
    <TooltipActionButtonBase
      icon={
        processing ? (
          <CircularProgress size={24} color="primary" disableShrink />
        ) : (
          icon
        )
      }
      title={title}
      {...args}
      action={async () => {
        setProcessing(true);
        await onClick();
        setProcessing(false);
      }}
      tooltipArgs={{
        placement: 'top',
        sx: {
          color: color ? color : 'text.icon',
          '&:hover': { bgcolor: 'transparent' }
        }
      }}
    />
  );
}

export default TooltipActionButton;
