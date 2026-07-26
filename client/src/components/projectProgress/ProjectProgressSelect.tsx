import StatusSelect, {
  type BoundStatusSelectProps
} from '@/components/status/StatusSelect';
import {
  projectProgresses,
  type ProjectProgress
} from '@/models/project/projectProgress';
import { projectProgressBadges } from './projectProgressBadges';

function ProjectProgressSelect(props: BoundStatusSelectProps<ProjectProgress>) {
  return (
    <StatusSelect
      {...props}
      options={projectProgresses}
      registry={projectProgressBadges}
    />
  );
}

export default ProjectProgressSelect;
