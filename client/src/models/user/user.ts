import { UserRole } from './userRole';

export type User = {
  id: string;
  name: string;
  isActivated: boolean;
  email: string;
  role: UserRole;
  registeredOn: Date;
  lastLoggedOn: Date;
  projects: string[];
  issues: string[];
  posts: string[];
};
