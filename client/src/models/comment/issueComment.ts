import { z } from 'zod';
import { CommentIdSchema, IssueIdSchema, UserIdSchema } from '@/models/ids';
import { optionalDate } from '@/models/primitives';

export const IssueCommentSchema = z.object({
  id: CommentIdSchema,
  issueId: IssueIdSchema,
  userId: UserIdSchema,
  content: z.string(),
  creationTime: optionalDate
});

export type IssueComment = z.infer<typeof IssueCommentSchema>;
