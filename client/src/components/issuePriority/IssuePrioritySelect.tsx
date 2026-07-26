import StatusSelect, {
  type BoundStatusSelectProps
} from '@/components/status/StatusSelect';
import {
  issuePriorities,
  type IssuePriority
} from '@/models/issue/issuePriority';
import { issuePriorityBadges } from './issuePriorityBadges';

function IssuePrioritySelect(props: BoundStatusSelectProps<IssuePriority>) {
  return (
    <StatusSelect
      {...props}
      options={issuePriorities}
      registry={issuePriorityBadges}
    />
  );
}

export default IssuePrioritySelect;
