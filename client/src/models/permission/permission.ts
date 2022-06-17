import { IssuePermission } from '../issue/issuePermission';

export type Permission = {
  userId: string;
  issueId: string;
  isPinnedToKanban: boolean;
  kanbanRowPosition: number;
  issuePermission: IssuePermission;
};
