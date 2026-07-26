import { z } from 'zod';
import { IssueCommentSchema } from '@/models/comment/issueComment';
import { IssueIdSchema, ProjectIdSchema, UserIdSchema } from '@/models/ids';
import { PermissionSchema } from '@/models/permission/permission';
import { optionalDate } from '@/models/primitives';
import { IssuePrioritySchema } from './issuePriority';
import { IssueProgressSchema } from './issueProgress';
import { IssueTypeSchema } from './issueType';

export const IssueSchema = z.object({
  id: IssueIdSchema,
  projectId: ProjectIdSchema,
  type: IssueTypeSchema,
  progress: IssueProgressSchema,
  priority: IssuePrioritySchema,
  permission: PermissionSchema.nullish().transform((v) => v ?? undefined),
  summary: z.string(),
  description: z
    .string()
    .nullish()
    .transform((v) => v ?? ''),
  createdBy: z.string(),
  responsibleBy: z
    .array(UserIdSchema)
    .nullish()
    .transform((v) => v ?? []),
  creationTime: optionalDate,
  completionTime: optionalDate,
  commentPageCount: z.number().int().nonnegative().catch(0),
  comments: z
    .array(IssueCommentSchema)
    .nullish()
    .transform((v) => v ?? [])
});

export type Issue = z.infer<typeof IssueSchema>;
