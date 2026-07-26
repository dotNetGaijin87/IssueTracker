import { z } from 'zod';

export const IssuePriority = {
  Low: 'Low',
  Medium: 'Medium',
  High: 'High',
  Critical: 'Critical'
} as const;

export type IssuePriority = (typeof IssuePriority)[keyof typeof IssuePriority];

export const IssuePrioritySchema = z.enum(IssuePriority);

export const issuePriorities: readonly IssuePriority[] =
  Object.values(IssuePriority);
