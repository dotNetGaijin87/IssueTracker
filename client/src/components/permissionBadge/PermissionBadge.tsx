import { IssuePermission } from '../../models/issue/issuePermission';
import permissionBadgeList from './permissionBadgeList';

interface Props {
  value: IssuePermission;
}
function PermissionBadge({ value }: Props): JSX.Element {
  return (
    permissionBadgeList().find((item: any) => item.value === value)
      ?.element ?? <div></div>
  );
}

export default PermissionBadge;
