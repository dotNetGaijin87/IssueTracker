import { IssueType } from './issueType';
import { IssuePriority } from './issuePriority';
import { IssueProgress } from './issueProgress';
import { IssueComment } from '../comment/issueComment';

export type Issue = {
  id: string;
  projectId: string;
  summary: string;
  description: string;
  type: IssueType;
  progress: IssueProgress;
  priority: IssuePriority;
  createdBy: string;
  responsibleBy: string[] | undefined;
  creationTime: Date | undefined;
  completionTime: Date | undefined;
  commentPageCount: number;
  comments: IssueComment[];
};

export const IssueDefaultValue: Issue = {
  id: '',
  projectId: '',
  summary: '',
  type: IssueType.Bug,
  progress: IssueProgress.ToDo,
  priority: IssuePriority.Low,
  description: '',
  createdBy: '',
  responsibleBy: [],
  creationTime: undefined,
  completionTime: undefined,
  commentPageCount: 0,
  comments: []
};
