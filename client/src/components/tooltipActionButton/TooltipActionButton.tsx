import { useState, type ReactNode } from 'react';
import { CircularProgress } from '@mui/material';
import TooltipActionButtonBase from './TooltipActionButtonBase';

interface Props {
  title: string;
  color?: string;
  icon: ReactNode;
  onClick: () => void | Promise<void>;
}

function TooltipActionButton({ title, color, icon, onClick }: Props) {
  const [processing, setProcessing] = useState(false);

  const handleClick = () => {
    setProcessing(true);
    void Promise.resolve(onClick()).finally(() => {
      setProcessing(false);
    });
  };

  return (
    <TooltipActionButtonBase
      title={title}
      icon={
        processing ? (
          <CircularProgress size={24} color="primary" disableShrink />
        ) : (
          icon
        )
      }
      onClick={handleClick}
      tooltipProps={{
        placement: 'top',
        sx: {
          color: color ?? 'text.icon',
          '&:hover': { bgcolor: 'transparent' }
        }
      }}
    />
  );
}

export default TooltipActionButton;
