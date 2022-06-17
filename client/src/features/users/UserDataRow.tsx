import { TableCell, TableRow, ToggleButton } from '@mui/material';
import { useAuth } from '../../authentication/Auth';
import safelyConvertDateTime from '../../helpers/time/safelyConvertDateTime';
import { User } from '../../models/user/user';
import { UserRole } from '../../models/user/userRole';
import CheckIcon from '@mui/icons-material/Check';
import CloseIcon from '@mui/icons-material/Close';
import React from 'react';
import { adapter } from '../../adapters/adapter';
import displayError from '../../helpers/errorHandling/displayError';
import { toast } from 'react-toastify';

interface Props {
  user: User;
}

function UserDataRow({ user }: Props) {
  const { authUser } = useAuth();
  const [userActivated, setUserActivated] = React.useState<boolean>(
    user.isActivated
  );

  const handleChangeUserActivationStatus = async () => {
    try {
      await adapter.User.update({ isActivated: !userActivated, Id: user.id });
      !userActivated
        ? toast.success('User account activated')
        : toast.success('User account deactivated');
      setUserActivated(!userActivated);
    } catch (ex) {
      displayError(ex, 'Saving data error');
    }
  };

  return (
    <TableRow key={user.id}>
      {authUser?.role === UserRole.admin && (
        <TableCell>
          <ToggleButton
            value="check"
            size="small"
            selected={userActivated}
            onChange={handleChangeUserActivationStatus}
          >
            {userActivated ? <CheckIcon /> : <CloseIcon />}
          </ToggleButton>
        </TableCell>
      )}

      <TableCell>{user.id}</TableCell>
      <TableCell>{user.email}</TableCell>
      <TableCell>{user.role}</TableCell>
      {authUser?.role === UserRole.admin && (
        <>
          <TableCell>{safelyConvertDateTime(user.registeredOn)}</TableCell>
          <TableCell>{safelyConvertDateTime(user.lastLoggedOn)}</TableCell>
        </>
      )}
    </TableRow>
  );
}

export default UserDataRow;
