import { IssuePriority } from '../issue/issuePriority';
import { IssueProgress } from '../issue/issueProgress';
import { IssueType } from '../issue/issueType';

export type KanbanCard = {
  id: string;
  projectId: string;
  summary: string;
  type: IssueType;
  progress: IssueProgress;
  priority: IssuePriority;
};
