import AssigneesSetter from '@/components/assigneesSetter/AssigneesSetter';
import Panel from '@/components/panel/Panel';
import type { Capabilities } from '@/models/access';
import type { Issue } from '@/models/issue/issue';

interface Props {
  issue: Issue;
  capabilities: Capabilities;
}

function Assignees({ issue, capabilities }: Props) {
  return (
    <Panel>
      <div style={{ width: '40%' }}>
        <AssigneesSetter
          issueId={issue.id}
          disabled={!capabilities.canModify}
        />
      </div>
    </Panel>
  );
}

export default Assignees;
