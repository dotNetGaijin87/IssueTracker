import AssigneesSetter from '../../../components/assigneesSetter/AssigneesSetter';
import Panel from '../../../components/panel/Panel';
import { Issue } from '../../../models/issue/issue';
import { IssuePermission } from '../../../models/issue/issuePermission';

interface Props {
  issue: Issue | undefined;
  permissions: IssuePermission[];
}

function Assignees({ issue, permissions }: Props) {
  const canModifyPermission = permissions.includes(IssuePermission.CanModify);

  return (
    <Panel>
      <div style={{ width: '40%' }}>
        <AssigneesSetter
          issueId={issue?.id}
          disabled={!canModifyPermission}
          onChange={(responsibleBy) => {}}
        />
      </div>
    </Panel>
  );
}

export default Assignees;
