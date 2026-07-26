import axios, { type AxiosRequestConfig } from 'axios';
import { z } from 'zod';
import { baseUrl } from '@/AppSettings';
import { getAccessToken } from '@/authentication/authClient';
import { ApiError, toApiError } from './apiError';

const REQUEST_TIMEOUT_MS = 5000;

export const http = axios.create({
  baseURL: baseUrl,
  timeout: REQUEST_TIMEOUT_MS
});

http.interceptors.request.use(async (config) => {
  const accessToken = await getAccessToken();
  if (accessToken !== undefined) {
    config.headers.set('Authorization', `Bearer ${accessToken}`);
  }
  return config;
});

http.interceptors.response.use(
  (response) => response,
  (error: unknown) => Promise.reject(toApiError(error, 'Request failed'))
);

export type QueryParams = Record<
  string,
  string | number | boolean | null | undefined
>;

type DefinedParams = Record<string, string | number | boolean>;

/** The API treats absent and empty filters alike; sending them narrows nothing. */
export function compactParams(params: QueryParams = {}): DefinedParams {
  const entries = Object.entries(params).filter(
    (entry): entry is [string, string | number | boolean] => {
      const value = entry[1];
      return value !== undefined && value !== null && value !== '';
    }
  );
  return Object.fromEntries(entries);
}

export function parseResponse<T>(
  schema: z.ZodType<T>,
  data: unknown,
  source: string
): T {
  const result = schema.safeParse(data);
  if (!result.success) {
    console.error(
      `Unexpected response shape from ${source}`,
      z.prettifyError(result.error),
      data
    );
    throw new ApiError(`The server returned unexpected data for ${source}.`);
  }
  return result.data;
}

async function send<T>(
  schema: z.ZodType<T>,
  config: AxiosRequestConfig
): Promise<T> {
  const response = await http.request<unknown>(config);
  return parseResponse(
    schema,
    response.data,
    `${config.method ?? 'GET'} ${config.url ?? ''}`
  );
}

export const request = {
  get: <T>(schema: z.ZodType<T>, url: string, params?: QueryParams) =>
    send(schema, { method: 'GET', url, params: compactParams(params) }),

  post: <T>(schema: z.ZodType<T>, url: string, data: unknown) =>
    send(schema, { method: 'POST', url, data }),

  patch: <T>(schema: z.ZodType<T>, url: string, data: unknown) =>
    send(schema, { method: 'PATCH', url, data }),

  remove: async (url: string): Promise<void> => {
    await http.request({ method: 'DELETE', url });
  }
};
