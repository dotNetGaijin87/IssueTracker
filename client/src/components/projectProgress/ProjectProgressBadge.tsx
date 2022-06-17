import { ProjectProgress } from '../../models/project/projectProgress';
import projectProgressList from './projectProgress/projectProgressList';

interface Props {
  value?: ProjectProgress;
}
function ProjectProgressBadge({ value }: Props): JSX.Element {
  return (
    projectProgressList(false).find((item: any) => item.value === value)
      ?.element ?? <div></div>
  );
}

export default ProjectProgressBadge;
