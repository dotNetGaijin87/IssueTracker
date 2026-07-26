import { z } from 'zod';

export const UserRole = {
  employee: 'employee',
  manager: 'manager',
  admin: 'admin'
} as const;

export type UserRole = (typeof UserRole)[keyof typeof UserRole];

export const UserRoleSchema = z.enum(UserRole);

export const userRoles: readonly UserRole[] = Object.values(UserRole);
