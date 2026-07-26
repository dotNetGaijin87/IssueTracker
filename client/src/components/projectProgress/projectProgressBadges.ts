import type { BadgeRegistry } from '@/components/status/badgeRegistry';
import { ProjectProgress } from '@/models/project/projectProgress';

export const projectProgressBadges: BadgeRegistry<ProjectProgress> = {
  [ProjectProgress.Open]: { color: '#b75709', backgroundColor: '#b7570913' },
  [ProjectProgress.Closed]: { color: '#0e9a31', backgroundColor: '#0e9a3116' }
};
