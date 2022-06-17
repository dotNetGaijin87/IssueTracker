import React from 'react';
import { Box, Button, Dialog, DialogActions, DialogTitle } from '@mui/material';
import { LoadingButton } from '@mui/lab';
import TooltipActionButton from '../tooltipActionButton/TooltipActionButton';

interface Props {
  hoverOverTitle: string;
  dialogText: string;
  icon: JSX.Element;
  onConfirm: () => void;
}

function ButtonIconWithConfirmationDialog({
  hoverOverTitle,
  dialogText,
  icon,
  onConfirm
}: Props) {
  const [dialogOpen, setDialogOpen] = React.useState(false);
  const [processing, setProcessing] = React.useState(false);
  const handleClose = () => {
    setDialogOpen(false);
  };

  const handleOpen = () => {
    setDialogOpen(true);
  };

  const handleConfirm = async () => {
    setProcessing(true);
    await onConfirm();
    setProcessing(false);
  };

  return (
    <>
      <Box component="span">
        <TooltipActionButton
          title={hoverOverTitle}
          icon={icon}
          onClick={handleOpen}
        />
      </Box>
      <Dialog open={dialogOpen} onClose={handleClose}>
        <DialogTitle>{dialogText}</DialogTitle>
        <DialogActions>
          <Button variant="text" color="secondary" onClick={handleClose}>
            CANCEL
          </Button>
          <LoadingButton
            variant="text"
            color="secondary"
            loading={processing}
            onClick={handleConfirm}
          >
            OK
          </LoadingButton>
        </DialogActions>
      </Dialog>
    </>
  );
}

export default ButtonIconWithConfirmationDialog;
