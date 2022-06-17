import { Issue } from '../issue/issue';
import { ProjectProgress } from './projectProgress';

export type Project = {
  id: string;
  summary: string;
  description: string;
  createdBy: string;
  creationTime: Date | undefined;
  completionTime: Date | undefined;
  progress: ProjectProgress;
  issues: Issue[] | undefined;
};

export const ProjectDefaultValue: Project = {
  id: '',
  summary: '',
  description: '',
  createdBy: '',
  creationTime: undefined,
  completionTime: undefined,
  progress: ProjectProgress.Open,
  issues: undefined
};
