import PestControlIcon from '@mui/icons-material/PestControl';
import ShowChartIcon from '@mui/icons-material/ShowChart';
import type { BadgeRegistry } from '@/components/status/badgeRegistry';
import { IssueType } from '@/models/issue/issueType';

export const issueTypeBadges: BadgeRegistry<IssueType> = {
  [IssueType.Bug]: {
    color: '#f24e34',
    backgroundColor: 'transparent',
    icon: <PestControlIcon sx={{ color: '#ffffff20' }} />
  },
  [IssueType.Improvement]: {
    color: '#7fb115',
    backgroundColor: 'transparent',
    icon: <ShowChartIcon sx={{ color: '#ffffff20' }} />
  }
};
