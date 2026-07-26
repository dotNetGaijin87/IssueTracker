import { z } from 'zod';

export const IssueIdSchema = z.string().min(1).brand<'IssueId'>();
export const ProjectIdSchema = z.string().min(1).brand<'ProjectId'>();
export const UserIdSchema = z.string().min(1).brand<'UserId'>();
export const CommentIdSchema = z.string().min(1).brand<'CommentId'>();

export type IssueId = z.infer<typeof IssueIdSchema>;
export type ProjectId = z.infer<typeof ProjectIdSchema>;
export type UserId = z.infer<typeof UserIdSchema>;
export type CommentId = z.infer<typeof CommentIdSchema>;

export const toIssueId = (value: string): IssueId => IssueIdSchema.parse(value);
export const toProjectId = (value: string): ProjectId =>
  ProjectIdSchema.parse(value);
export const toUserId = (value: string): UserId => UserIdSchema.parse(value);
export const toCommentId = (value: string): CommentId =>
  CommentIdSchema.parse(value);
