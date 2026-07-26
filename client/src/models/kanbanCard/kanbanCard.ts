import { z } from 'zod';
import { IssueIdSchema, ProjectIdSchema } from '@/models/ids';
import { IssuePrioritySchema } from '@/models/issue/issuePriority';
import { IssueProgressSchema } from '@/models/issue/issueProgress';
import { IssueTypeSchema } from '@/models/issue/issueType';

export const KanbanCardSchema = z.object({
  id: IssueIdSchema,
  projectId: ProjectIdSchema,
  position: z.number().int(),
  summary: z
    .string()
    .nullish()
    .transform((v) => v ?? ''),
  type: IssueTypeSchema,
  progress: IssueProgressSchema,
  priority: IssuePrioritySchema
});

export type KanbanCard = z.infer<typeof KanbanCardSchema>;
