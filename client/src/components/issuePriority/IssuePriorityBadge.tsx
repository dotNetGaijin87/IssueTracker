import { IssuePriority } from '../../models/issue/issuePriority';
import issuePriorityList from './issuePriority/issuePriorityList';

interface Props {
  value?: IssuePriority;
  unstyled?: boolean;
  borderless?: boolean;
}
function IssuePriorityBadge({
  value,
  unstyled,
  borderless
}: Props): JSX.Element {
  return (
    issuePriorityList(unstyled, borderless).find(
      (item: any) => item.value === value
    )?.element ?? <div></div>
  );
}

export default IssuePriorityBadge;
