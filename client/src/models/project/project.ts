import { z } from 'zod';
import { IssueSchema } from '@/models/issue/issue';
import { ProjectIdSchema } from '@/models/ids';
import { optionalDate } from '@/models/primitives';
import { ProjectProgressSchema } from './projectProgress';

export const ProjectSchema = z.object({
  id: ProjectIdSchema,
  summary: z.string(),
  description: z
    .string()
    .nullish()
    .transform((v) => v ?? ''),
  createdBy: z.string(),
  ownedBy: z
    .string()
    .nullish()
    .transform((v) => v ?? ''),
  progress: ProjectProgressSchema,
  creationTime: optionalDate,
  completionTime: optionalDate,
  issues: z
    .array(IssueSchema)
    .nullish()
    .transform((v) => v ?? [])
});

export type Project = z.infer<typeof ProjectSchema>;
