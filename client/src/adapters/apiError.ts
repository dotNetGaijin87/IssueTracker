import { z } from 'zod';

/** `Hellang.Middleware.ProblemDetails` shape returned by the API on failure. */
const ProblemDetailsSchema = z.object({
  title: z.string().optional(),
  detail: z.string().optional(),
  status: z.number().optional()
});

export class ApiError extends Error {
  readonly status: number | undefined;

  constructor(message: string, status?: number) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
  }
}

export function isApiError(error: unknown): error is ApiError {
  return error instanceof ApiError;
}

function hasResponseData(error: unknown): error is {
  response?: { status?: number; data?: unknown };
  message?: string;
} {
  return typeof error === 'object' && error !== null;
}

/** Normalises anything thrown by axios, zod or user code into an `ApiError`. */
export function toApiError(error: unknown, fallbackMessage: string): ApiError {
  if (isApiError(error)) return error;

  if (hasResponseData(error)) {
    const problem = ProblemDetailsSchema.safeParse(error.response?.data);
    if (problem.success && problem.data.detail) {
      return new ApiError(problem.data.detail, error.response?.status);
    }
    if (typeof error.message === 'string' && error.message.length > 0) {
      return new ApiError(error.message, error.response?.status);
    }
  }

  if (error instanceof Error && error.message.length > 0) {
    return new ApiError(error.message);
  }

  return new ApiError(fallbackMessage);
}

export function errorMessage(error: unknown, fallbackMessage: string): string {
  return toApiError(error, fallbackMessage).message;
}
