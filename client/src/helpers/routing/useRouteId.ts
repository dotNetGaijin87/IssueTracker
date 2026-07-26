import { useParams } from 'react-router-dom';
import {
  IssueIdSchema,
  ProjectIdSchema,
  type IssueId,
  type ProjectId
} from '@/models/ids';

/** Route params are strings; branding them here keeps the casts at the edge. */
export function useProjectId(): ProjectId | undefined {
  const { projectId } = useParams<{ projectId: string }>();
  const result = ProjectIdSchema.safeParse(projectId);
  return result.success ? result.data : undefined;
}

export function useIssueId(): IssueId | undefined {
  const { issueId } = useParams<{ issueId: string }>();
  const result = IssueIdSchema.safeParse(issueId);
  return result.success ? result.data : undefined;
}
