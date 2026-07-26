import type { BadgeRegistry } from '@/components/status/badgeRegistry';
import { IssueProgress } from '@/models/issue/issueProgress';

export const issueProgressBadges: BadgeRegistry<IssueProgress> = {
  [IssueProgress.ToDo]: { color: '#137ac3', backgroundColor: '#50a7e61a' },
  [IssueProgress.InProgress]: {
    color: '#c96915',
    backgroundColor: '#c9691540'
  },
  [IssueProgress.ToBeTested]: {
    color: '#c134bc',
    backgroundColor: '#e622df30'
  },
  [IssueProgress.Closed]: { color: '#44a90d', backgroundColor: '#64ce2c0a' },
  [IssueProgress.OnHold]: { color: '#9d7905', backgroundColor: '#9d79053d' },
  [IssueProgress.Canceled]: { color: '#f85149', backgroundColor: '#f851490f' }
};
