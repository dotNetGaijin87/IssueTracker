import { UserRole } from '../../models/user/userRole';

function hasAdminOrManagerRole(userRole: UserRole | undefined): boolean {
  if (userRole)
    return userRole === UserRole.admin || userRole === UserRole.manager;
  else return false;
}

export default hasAdminOrManagerRole;
