import { z } from 'zod';

export const ProjectProgress = {
  Open: 'Open',
  Closed: 'Closed'
} as const;

export type ProjectProgress =
  (typeof ProjectProgress)[keyof typeof ProjectProgress];

export const ProjectProgressSchema = z.enum(ProjectProgress);

export const projectProgresses: readonly ProjectProgress[] =
  Object.values(ProjectProgress);
