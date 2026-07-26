import { UserIdSchema, type UserId } from '@/models/ids';

/** Auth0 subjects look like `auth0|1a2b3c`; the API stores only the suffix. */
function extractUserId(subject: string | undefined): UserId | undefined {
  const result = UserIdSchema.safeParse(subject?.split('|').pop());
  return result.success ? result.data : undefined;
}

export default extractUserId;
