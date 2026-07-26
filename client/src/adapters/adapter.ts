import { z } from 'zod';
import getFieldMask from '@/helpers/getFieldMask';
import { IssueCommentSchema } from '@/models/comment/issueComment';
import type { CommentId, IssueId, ProjectId, UserId } from '@/models/ids';
import { IssueSchema } from '@/models/issue/issue';
import type { IssuePermission } from '@/models/issue/issuePermission';
import type { IssuePriority } from '@/models/issue/issuePriority';
import type { IssueProgress } from '@/models/issue/issueProgress';
import type { IssueType } from '@/models/issue/issueType';
import { KanbanCardSchema } from '@/models/kanbanCard/kanbanCard';
import { paginatedSchema } from '@/models/pagination';
import { PermissionSchema } from '@/models/permission/permission';
import { ProjectSchema } from '@/models/project/project';
import type { ProjectProgress } from '@/models/project/projectProgress';
import { UserSchema } from '@/models/user/user';
import { request } from './http';

const IssueListSchema = paginatedSchema('issues', IssueSchema);
const ProjectListSchema = paginatedSchema('projects', ProjectSchema);
const UserListSchema = paginatedSchema('users', UserSchema);
const CommentListSchema = paginatedSchema('comments', IssueCommentSchema);
const PermissionListSchema = paginatedSchema('permissions', PermissionSchema);
const KanbanCardsSchema = z.array(KanbanCardSchema);

type PageCriteria = {
  page?: number | undefined;
  pageSize?: number | undefined;
};

function withFieldMask<T extends Record<string, unknown>>(data: T) {
  return { FieldMask: getFieldMask(data), ...data };
}

export type IssueListCriteria = PageCriteria & {
  projectId?: ProjectId | undefined;
  id?: string | undefined;
  createdBy?: string | undefined;
  ownerId?: UserId | undefined;
  type?: IssueType | undefined;
  progress?: IssueProgress | undefined;
  priority?: IssuePriority | undefined;
};

export type CreateIssueRequest = {
  id: string;
  projectId: ProjectId;
  summary: string;
  description: string;
  type: IssueType;
  progress: IssueProgress;
  priority: IssuePriority;
  responsibleBy: string[];
};

export type UpdateIssueRequest = {
  id: IssueId;
  summary: string;
  description: string;
  type: IssueType;
  progress: IssueProgress;
  priority: IssuePriority;
};

export type KanbanPositionUpdate = {
  issueId: IssueId;
  kanbanRowPosition: number;
  isPinnedToKanban: boolean;
};

export type UpdateKanbanRequest = {
  issueId: IssueId;
  progress: IssueProgress;
  permissions: KanbanPositionUpdate[];
};

const Issue = {
  get: (id: IssueId) => request.get(IssueSchema, `issue/${id}`),
  list: (criteria: IssueListCriteria = {}) =>
    request.get(IssueListSchema, 'issue', criteria),
  create: (data: CreateIssueRequest) =>
    request.post(IssueSchema, 'issue', data),
  update: (data: UpdateIssueRequest) =>
    request.patch(IssueSchema, 'issue', withFieldMask(data)),
  delete: (id: IssueId) => request.remove(`issue/${id}`),
  getKanban: () => request.post(KanbanCardsSchema, 'issue/:getIssueKanban', {}),
  updateKanban: (data: UpdateKanbanRequest) =>
    request.post(KanbanCardsSchema, 'issue/:updateIssueKanban', data)
};

export type ProjectListCriteria = PageCriteria & {
  id?: string | undefined;
  createdBy?: string | undefined;
  progress?: ProjectProgress | undefined;
};

export type CreateProjectRequest = {
  id: string;
  summary: string;
  description: string;
  progress: ProjectProgress;
};

export type UpdateProjectRequest = {
  id: ProjectId;
  summary: string;
  description: string;
  progress: ProjectProgress;
};

const Project = {
  get: (id: ProjectId) => request.get(ProjectSchema, `project/${id}`),
  list: (criteria: ProjectListCriteria = {}) =>
    request.get(ProjectListSchema, 'project', criteria),
  create: (data: CreateProjectRequest) =>
    request.post(ProjectSchema, 'project', data),
  update: (data: UpdateProjectRequest) =>
    request.patch(ProjectSchema, 'project', withFieldMask(data)),
  delete: (id: ProjectId) => request.remove(`project/${id}`)
};

export type UserListCriteria = PageCriteria & {
  id?: string | undefined;
  email?: string | undefined;
};

export type UpdateUserRequest = {
  id: UserId;
  isActivated: boolean;
};

const User = {
  list: (criteria: UserListCriteria = {}) =>
    request.get(UserListSchema, 'user', criteria),
  update: (data: UpdateUserRequest) =>
    request.patch(UserSchema, 'user', withFieldMask(data)),
  /** Idempotent server-side upsert; identity comes from the bearer token. */
  createSafely: () => request.post(UserSchema, 'user/:createUserSafely', {})
};

export type PermissionListCriteria = PageCriteria & {
  userId?: UserId | undefined;
  issueId?: IssueId | undefined;
};

export type PermissionRequest = {
  userId: UserId;
  issueId: IssueId;
  isPinnedToKanban: boolean;
  issuePermission: IssuePermission;
};

const Permission = {
  get: (userId: UserId, issueId: IssueId) =>
    request.get(PermissionSchema, `permission/${userId}/${issueId}`),
  list: (criteria: PermissionListCriteria = {}) =>
    request.get(PermissionListSchema, 'permission', criteria),
  create: (data: PermissionRequest) =>
    request.post(PermissionSchema, 'permission', data),
  update: (data: PermissionRequest) =>
    request.patch(PermissionSchema, 'permission', data),
  delete: (userId: UserId, issueId: IssueId) =>
    request.remove(`permission/${userId}/${issueId}`)
};

export type CommentListCriteria = PageCriteria & {
  issueId?: IssueId | undefined;
  content?: string | undefined;
  createdBy?: string | undefined;
};

export type CreateCommentRequest = {
  userId: UserId;
  issueId: IssueId;
  content: string;
};

export type UpdateCommentRequest = {
  id: CommentId;
  content: string;
};

const Comment = {
  list: (criteria: CommentListCriteria = {}) =>
    request.get(CommentListSchema, 'comment', criteria),
  create: (data: CreateCommentRequest) =>
    request.post(IssueCommentSchema, 'comment', data),
  update: (data: UpdateCommentRequest) =>
    request.patch(IssueCommentSchema, 'comment', data),
  delete: (id: CommentId) => request.remove(`comment/${id}`)
};

export const adapter = {
  User,
  Project,
  Issue,
  Permission,
  Comment
};
