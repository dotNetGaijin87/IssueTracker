import { z } from 'zod';

export const IssuePermission = {
  None: 'None',
  CanModify: 'CanModify',
  CanDelete: 'CanDelete'
} as const;

export type IssuePermission =
  (typeof IssuePermission)[keyof typeof IssuePermission];

export const IssuePermissionSchema = z.enum(IssuePermission);
