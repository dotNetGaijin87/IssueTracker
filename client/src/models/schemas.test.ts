import { describe, expect, it } from 'vitest';
import { IssueSchema } from './issue/issue';
import { IssuePermissionSchema } from './issue/issuePermission';
import { paginatedSchema } from './pagination';
import { UserSchema } from './user/user';

/** Mirrors an `IssueVm` as ASP.NET serialises it. */
const issueJson = {
  id: 'ISSUE-1',
  projectId: 'PROJ-1',
  type: 'Bug',
  progress: 'ToDo',
  priority: 'Low',
  permission: null,
  summary: 'Something is broken',
  description: 'details',
  createdBy: 'alice',
  creationTime: '2024-01-02T03:04:05Z',
  completionTime: null,
  commentPageCount: 0,
  comments: []
};

describe('IssueSchema', () => {
  it('coerces the ISO creationTime into a real Date', () => {
    const issue = IssueSchema.parse(issueJson);
    expect(issue.creationTime).toBeInstanceOf(Date);
    expect(issue.creationTime?.toISOString()).toBe('2024-01-02T03:04:05.000Z');
  });

  it('maps a null completionTime to undefined rather than the epoch', () => {
    expect(IssueSchema.parse(issueJson).completionTime).toBeUndefined();
  });

  it('defaults absent collections instead of throwing', () => {
    // `responsibleBy` is never sent by IssueVm and `comments` may be omitted.
    const { comments: _comments, ...withoutCollections } = issueJson;

    const issue = IssueSchema.parse(withoutCollections);
    expect(issue.comments).toEqual([]);
    expect(issue.responsibleBy).toEqual([]);
  });

  it('rejects a status the server could never send', () => {
    expect(
      IssueSchema.safeParse({ ...issueJson, progress: 'Unspecified' }).success
    ).toBe(false);
  });

  it('rejects an empty id', () => {
    expect(IssueSchema.safeParse({ ...issueJson, id: '' }).success).toBe(false);
  });
});

describe('IssuePermissionSchema', () => {
  it('accepts the server-side None member', () => {
    expect(IssuePermissionSchema.parse('None')).toBe('None');
  });

  it('rejects the client-only CanSee member that no longer exists', () => {
    expect(IssuePermissionSchema.safeParse('CanSee').success).toBe(false);
  });
});

describe('UserSchema', () => {
  it('strips the nested graphs the client does not model', () => {
    const user = UserSchema.parse({
      id: 'alice',
      name: 'Alice',
      email: 'alice@example.com',
      imageUrl: null,
      isActivated: true,
      role: 'admin',
      registeredOn: '2024-01-01T00:00:00Z',
      lastLoggedOn: '2024-05-01T00:00:00Z',
      projects: [{ id: 'p1' }],
      issues: [{ userId: 'alice' }],
      posts: [{ id: 'c1' }]
    });

    expect(user).not.toHaveProperty('projects');
    expect(user.imageUrl).toBe('');
    expect(user.role).toBe('admin');
  });
});

describe('paginatedSchema', () => {
  const schema = paginatedSchema('issues', IssueSchema);

  it('normalises the resource-specific key to items', () => {
    const page = schema.parse({ issues: [issueJson], page: 2, pageCount: 7 });
    expect(page.items).toHaveLength(1);
    expect(page.page).toBe(2);
    expect(page.pageCount).toBe(7);
  });

  it('survives an envelope with no collection at all', () => {
    expect(schema.parse({ page: 1, pageCount: 0 }).items).toEqual([]);
  });

  it('falls back to sane paging numbers when they are missing', () => {
    const page = schema.parse({ issues: [] });
    expect(page.page).toBe(1);
    expect(page.pageCount).toBe(0);
  });
});
