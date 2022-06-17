import { IssueProgress } from '../../models/issue/issueProgress';
import issueProgressList from './issueProgress/issueProgressList';

interface Props {
  value?: IssueProgress;
}
function IssueProgressBadge({ value }: Props): JSX.Element {
  return (
    issueProgressList(false).find((item: any) => item.value === value)
      ?.element ?? <div></div>
  );
}

export default IssueProgressBadge;
