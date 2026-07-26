import { z } from 'zod';
import { UserIdSchema } from '@/models/ids';
import { optionalDate } from '@/models/primitives';
import { UserRoleSchema } from './userRole';

/**
 * The server also returns nested `projects`, `issues` and `posts` graphs.
 * They are unused by the client, so they are stripped rather than modelled.
 */
export const UserSchema = z.object({
  id: UserIdSchema,
  name: z
    .string()
    .nullish()
    .transform((v) => v ?? ''),
  email: z
    .string()
    .nullish()
    .transform((v) => v ?? ''),
  imageUrl: z
    .string()
    .nullish()
    .transform((v) => v ?? ''),
  isActivated: z.boolean(),
  role: UserRoleSchema,
  registeredOn: optionalDate,
  lastLoggedOn: optionalDate
});

export type User = z.infer<typeof UserSchema>;
