import type { User as Auth0User } from '@auth0/auth0-spa-js';
import { z } from 'zod';
import { UserRoleSchema, type UserRole } from '@/models/user/userRole';

const ROLES_CLAIM = 'http://namespace//roles';

const RolesClaimSchema = z.array(UserRoleSchema).nonempty();

function extractUserRole(user: Auth0User | undefined): UserRole | undefined {
  const result = RolesClaimSchema.safeParse(user?.[ROLES_CLAIM]);
  return result.success ? result.data[0] : undefined;
}

export default extractUserRole;
