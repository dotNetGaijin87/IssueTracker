import StatusSelect, {
  type BoundStatusSelectProps
} from '@/components/status/StatusSelect';
import { issueTypes, type IssueType } from '@/models/issue/issueType';
import { issueTypeBadges } from './issueTypeBadges';

function IssueTypeSelect(props: BoundStatusSelectProps<IssueType>) {
  return (
    <StatusSelect {...props} options={issueTypes} registry={issueTypeBadges} />
  );
}

export default IssueTypeSelect;
