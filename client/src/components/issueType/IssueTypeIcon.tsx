import { IssueType } from '../../models/issue/issueType';
import issueTypeList from './issueType/issueTypeList';

interface Props {
  value?: IssueType;
}
function IssueTypeIcon({ value }: Props): JSX.Element {
  return (
    issueTypeList(false).find((item: any) => item.value === value)?.icon ?? (
      <div></div>
    )
  );
}

export default IssueTypeIcon;
