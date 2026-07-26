import { useState } from 'react';
import { TableCell, TableRow, ToggleButton } from '@mui/material';
import CheckIcon from '@mui/icons-material/Check';
import CloseIcon from '@mui/icons-material/Close';
import { toast } from 'react-toastify';
import { adapter } from '@/adapters/adapter';
import { useAuth } from '@/authentication/Auth';
import displayError from '@/helpers/errorHandling/displayError';
import safelyConvertDateTime from '@/helpers/time/safelyConvertDateTime';
import type { User } from '@/models/user/user';
import { UserRole } from '@/models/user/userRole';

interface Props {
  user: User;
}

function UserDataRow({ user }: Props) {
  const { authUser } = useAuth();
  const [activated, setActivated] = useState(user.isActivated);
  const isAdmin = authUser?.role === UserRole.admin;

  const handleActivationToggle = async () => {
    const next = !activated;

    try {
      await adapter.User.update({ id: user.id, isActivated: next });
      setActivated(next);
      toast.success(
        next ? 'User account activated' : 'User account deactivated'
      );
    } catch (error) {
      displayError(error, 'Saving data error');
    }
  };

  return (
    <TableRow>
      {isAdmin && (
        <TableCell>
          <ToggleButton
            value="check"
            size="small"
            selected={activated}
            onChange={() => {
              void handleActivationToggle();
            }}
          >
            {activated ? <CheckIcon /> : <CloseIcon />}
          </ToggleButton>
        </TableCell>
      )}

      <TableCell>{user.id}</TableCell>
      <TableCell>{user.email}</TableCell>
      <TableCell>{user.role}</TableCell>
      {isAdmin && (
        <>
          <TableCell>{safelyConvertDateTime(user.registeredOn)}</TableCell>
          <TableCell>{safelyConvertDateTime(user.lastLoggedOn)}</TableCell>
        </>
      )}
    </TableRow>
  );
}

export default UserDataRow;
