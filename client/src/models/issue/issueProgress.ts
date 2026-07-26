import { z } from 'zod';

export const IssueProgress = {
  ToDo: 'ToDo',
  InProgress: 'InProgress',
  ToBeTested: 'ToBeTested',
  Closed: 'Closed',
  OnHold: 'OnHold',
  Canceled: 'Canceled'
} as const;

export type IssueProgress = (typeof IssueProgress)[keyof typeof IssueProgress];

export const IssueProgressSchema = z.enum(IssueProgress);

export const issueProgresses: readonly IssueProgress[] =
  Object.values(IssueProgress);
