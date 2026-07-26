import StatusBadge, {
  type BoundStatusBadgeProps
} from '@/components/status/StatusBadge';
import type { IssueType } from '@/models/issue/issueType';
import { issueTypeBadges } from './issueTypeBadges';

function IssueTypeBadge(props: BoundStatusBadgeProps<IssueType>) {
  return <StatusBadge {...props} registry={issueTypeBadges} />;
}

export default IssueTypeBadge;
