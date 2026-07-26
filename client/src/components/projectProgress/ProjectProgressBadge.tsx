import StatusBadge, {
  type BoundStatusBadgeProps
} from '@/components/status/StatusBadge';
import type { ProjectProgress } from '@/models/project/projectProgress';
import { projectProgressBadges } from './projectProgressBadges';

function ProjectProgressBadge(props: BoundStatusBadgeProps<ProjectProgress>) {
  return <StatusBadge {...props} registry={projectProgressBadges} />;
}

export default ProjectProgressBadge;
