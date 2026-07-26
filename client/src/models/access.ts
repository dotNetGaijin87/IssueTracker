import { IssuePermission } from '@/models/issue/issuePermission';
import { UserRole } from '@/models/user/userRole';

export type Capabilities = {
  canModify: boolean;
  canDelete: boolean;
};

export const noCapabilities: Capabilities = {
  canModify: false,
  canDelete: false
};

export function isPrivilegedRole(role: UserRole | undefined): boolean {
  return role === UserRole.admin || role === UserRole.manager;
}

/**
 * `CanDelete` subsumes `CanModify`; expressing that once here stops every
 * caller from re-deriving the hierarchy by hand.
 */
export function issueCapabilities(input: {
  permission?: IssuePermission | undefined;
  role?: UserRole | undefined;
  isOwner?: boolean;
}): Capabilities {
  const permission = input.permission ?? IssuePermission.None;
  const privileged = isPrivilegedRole(input.role);
  const canDelete =
    privileged ||
    (input.isOwner ?? false) ||
    permission === IssuePermission.CanDelete;

  return {
    canDelete,
    canModify: canDelete || permission === IssuePermission.CanModify
  };
}

export function projectCapabilities(input: {
  role?: UserRole | undefined;
  isOwner?: boolean;
}): Capabilities {
  const allowed = input.role === UserRole.admin || (input.isOwner ?? false);
  return { canModify: allowed, canDelete: allowed };
}
