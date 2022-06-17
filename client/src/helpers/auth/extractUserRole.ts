import { User } from '@auth0/auth0-spa-js';
import { UserRole } from '../../models/user/userRole';

function extractUserRole(user: User | undefined): UserRole {
  if (user === undefined) {
    new Error('No user role');
  }
  let roleStr: string[] = user!['http://namespace//roles'];

  if (roleStr === undefined) {
    new Error('No user role');
  }

  var role: UserRole = <UserRole>roleStr[0];

  return role;
}

export default extractUserRole;
