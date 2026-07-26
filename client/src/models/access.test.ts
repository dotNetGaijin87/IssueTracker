import { describe, expect, it } from 'vitest';
import {
  isPrivilegedRole,
  issueCapabilities,
  projectCapabilities
} from './access';
import { IssuePermission } from './issue/issuePermission';
import { UserRole } from './user/userRole';

describe('isPrivilegedRole', () => {
  it('accepts admins and managers', () => {
    expect(isPrivilegedRole(UserRole.admin)).toBe(true);
    expect(isPrivilegedRole(UserRole.manager)).toBe(true);
  });

  it('rejects employees and unknown roles', () => {
    expect(isPrivilegedRole(UserRole.employee)).toBe(false);
    expect(isPrivilegedRole(undefined)).toBe(false);
  });
});

describe('issueCapabilities', () => {
  it('grants nothing without a permission, role or ownership', () => {
    expect(issueCapabilities({})).toEqual({
      canModify: false,
      canDelete: false
    });
  });

  it('treats an explicit None permission as no access', () => {
    expect(
      issueCapabilities({
        permission: IssuePermission.None,
        role: UserRole.employee
      })
    ).toEqual({ canModify: false, canDelete: false });
  });

  it('lets CanModify modify but not delete', () => {
    expect(
      issueCapabilities({ permission: IssuePermission.CanModify })
    ).toEqual({ canModify: true, canDelete: false });
  });

  it('makes CanDelete subsume CanModify', () => {
    expect(
      issueCapabilities({ permission: IssuePermission.CanDelete })
    ).toEqual({ canModify: true, canDelete: true });
  });

  it.each([UserRole.admin, UserRole.manager])(
    'grants %s full access regardless of permission',
    (role) => {
      expect(issueCapabilities({ role })).toEqual({
        canModify: true,
        canDelete: true
      });
    }
  );

  it('grants the owner full access', () => {
    expect(
      issueCapabilities({ role: UserRole.employee, isOwner: true })
    ).toEqual({ canModify: true, canDelete: true });
  });

  it('does not let a privileged role be masked by a weaker permission', () => {
    expect(
      issueCapabilities({
        permission: IssuePermission.CanModify,
        role: UserRole.manager
      })
    ).toEqual({ canModify: true, canDelete: true });
  });
});

describe('projectCapabilities', () => {
  it('grants admins full access', () => {
    expect(projectCapabilities({ role: UserRole.admin })).toEqual({
      canModify: true,
      canDelete: true
    });
  });

  it('grants the owner full access', () => {
    expect(
      projectCapabilities({ role: UserRole.employee, isOwner: true })
    ).toEqual({ canModify: true, canDelete: true });
  });

  it('denies managers who do not own the project', () => {
    expect(projectCapabilities({ role: UserRole.manager })).toEqual({
      canModify: false,
      canDelete: false
    });
  });
});
