import { z } from 'zod';

export const IssueType = {
  Bug: 'Bug',
  Improvement: 'Improvement'
} as const;

export type IssueType = (typeof IssueType)[keyof typeof IssueType];

export const IssueTypeSchema = z.enum(IssueType);

export const issueTypes: readonly IssueType[] = Object.values(IssueType);
