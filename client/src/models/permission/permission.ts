import { z } from 'zod';
import { IssuePermissionSchema } from '@/models/issue/issuePermission';
import { IssueIdSchema, UserIdSchema } from '@/models/ids';

export const PermissionSchema = z.object({
  userId: UserIdSchema,
  issueId: IssueIdSchema,
  isPinnedToKanban: z.boolean(),
  kanbanRowPosition: z.number().int(),
  issuePermission: IssuePermissionSchema
});

export type Permission = z.infer<typeof PermissionSchema>;
