import StatusSelect, {
  type BoundStatusSelectProps
} from '@/components/status/StatusSelect';
import {
  issueProgresses,
  type IssueProgress
} from '@/models/issue/issueProgress';
import { issueProgressBadges } from './issueProgressBadges';

function IssueProgressSelect(props: BoundStatusSelectProps<IssueProgress>) {
  return (
    <StatusSelect
      {...props}
      options={issueProgresses}
      registry={issueProgressBadges}
    />
  );
}

export default IssueProgressSelect;
