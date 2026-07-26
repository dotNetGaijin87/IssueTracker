import type { IssueType } from '@/models/issue/issueType';
import { issueTypeBadges } from './issueTypeBadges';

interface Props {
  value: IssueType | undefined;
}

function IssueTypeIcon({ value }: Props) {
  if (value === undefined) return null;
  return <>{issueTypeBadges[value].icon}</>;
}

export default IssueTypeIcon;
