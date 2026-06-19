import { IssuePriority } from '@/models/issue/issuePriority';
import { IssueProgress } from '@/models/issue/issueProgress';
import { IssueType } from '@/models/issue/issueType';

export type KanbanCard = {
  id: string;
  projectId: string;
  summary: string;
  type: IssueType;
  progress: IssueProgress;
  priority: IssuePriority;
};
