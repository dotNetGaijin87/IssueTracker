import {
  Fade,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow
} from '@mui/material';
import TableContainer from '../../components/tableContainer/TableContainer';
import { useAuth } from '../../authentication/Auth';
import { User } from '../../models/user/user';
import { UserRole } from '../../models/user/userRole';
import UserDataRow from './UserDataRow';

interface Props {
  users?: User[];
}

function UsersTable({ users }: Props) {
  const { authUser } = useAuth();

  return (
    <TableContainer>
      <Fade in={true}>
        <Table>
          <TableHead>
            <TableRow>
              {authUser?.role === UserRole.admin && (
                <TableCell align="center">Activation</TableCell>
              )}
              <TableCell align="center">Name</TableCell>
              <TableCell align="center">email</TableCell>
              <TableCell align="center">Role</TableCell>
              {authUser?.role === UserRole.admin && (
                <>
                  <TableCell align="center">Registration Time</TableCell>
                  <TableCell align="center">Last Login Time</TableCell>
                </>
              )}
            </TableRow>
          </TableHead>
          <TableBody>
            {users?.map((user: User) => (
              <UserDataRow key={user.id} user={user} />
            ))}
          </TableBody>
        </Table>
      </Fade>
    </TableContainer>
  );
}

export default UsersTable;
