import Looks3Icon from '@mui/icons-material/Looks3';
import Looks4Icon from '@mui/icons-material/Looks4';
import LooksOneIcon from '@mui/icons-material/LooksOne';
import LooksTwoIcon from '@mui/icons-material/LooksTwo';
import type { BadgeRegistry } from '@/components/status/badgeRegistry';
import { IssuePriority } from '@/models/issue/issuePriority';

export const issuePriorityBadges: BadgeRegistry<IssuePriority> = {
  [IssuePriority.Low]: {
    color: '#08b587',
    backgroundColor: '#08b58712',
    icon: <LooksOneIcon sx={{ color: '#7ee787' }} />
  },
  [IssuePriority.Medium]: {
    color: '#1b9ec5',
    backgroundColor: '#1b9dc510',
    icon: <LooksTwoIcon sx={{ color: '#39b2d6' }} />
  },
  [IssuePriority.High]: {
    color: '#ffa100',
    backgroundColor: '#ffa10029',
    icon: <Looks3Icon sx={{ color: '#cdc82a' }} />
  },
  [IssuePriority.Critical]: {
    color: '#d14038',
    backgroundColor: '#d1403818',
    icon: <Looks4Icon sx={{ color: '#d6625b' }} />
  }
};
