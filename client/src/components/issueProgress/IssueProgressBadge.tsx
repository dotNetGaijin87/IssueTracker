import StatusBadge, {
  type BoundStatusBadgeProps
} from '@/components/status/StatusBadge';
import type { IssueProgress } from '@/models/issue/issueProgress';
import { issueProgressBadges } from './issueProgressBadges';

function IssueProgressBadge(props: BoundStatusBadgeProps<IssueProgress>) {
  return <StatusBadge {...props} registry={issueProgressBadges} />;
}

export default IssueProgressBadge;
