import { IssueType } from '../../models/issue/issueType';
import issueTypeList from './issueType/issueTypeList';

interface Props {
  value?: IssueType;
  unstyled?: boolean;
}
function IssueTypeBadge({ value, unstyled }: Props): JSX.Element {
  return (
    issueTypeList(unstyled).find((item: any) => item.value === value)
      ?.element ?? <div></div>
  );
}

export default IssueTypeBadge;
