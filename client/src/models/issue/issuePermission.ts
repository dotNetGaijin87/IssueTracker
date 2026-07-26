import { z } from 'zod';

/** Mirrors `IssueTracker.Domain.Models.Enums.IssuePermission` on the server. */
export const IssuePermission = {
  None: 'None',
  CanModify: 'CanModify',
  CanDelete: 'CanDelete'
} as const;

export type IssuePermission =
  (typeof IssuePermission)[keyof typeof IssuePermission];

export const IssuePermissionSchema = z.enum(IssuePermission);
