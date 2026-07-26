import StatusBadge, {
  type BoundStatusBadgeProps
} from '@/components/status/StatusBadge';
import type { IssuePriority } from '@/models/issue/issuePriority';
import { issuePriorityBadges } from './issuePriorityBadges';

function IssuePriorityBadge(props: BoundStatusBadgeProps<IssuePriority>) {
  return <StatusBadge {...props} registry={issuePriorityBadges} />;
}

export default IssuePriorityBadge;
