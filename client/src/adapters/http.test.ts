import { describe, expect, it, vi } from 'vitest';
import { z } from 'zod';
import { ApiError } from './apiError';
import { compactParams, parseResponse } from './http';

describe('compactParams', () => {
  it('keeps meaningful values', () => {
    expect(compactParams({ page: 2, id: 'A', pinned: false })).toEqual({
      page: 2,
      id: 'A',
      pinned: false
    });
  });

  it('drops undefined, null and empty-string filters', () => {
    expect(
      compactParams({ id: '', createdBy: undefined, progress: null, page: 1 })
    ).toEqual({ page: 1 });
  });

  it('keeps zero, which is a real page value', () => {
    expect(compactParams({ page: 0 })).toEqual({ page: 0 });
  });

  it('handles a missing params object', () => {
    expect(compactParams()).toEqual({});
  });
});

describe('parseResponse', () => {
  const schema = z.object({ id: z.string() });

  it('returns parsed data on a match', () => {
    expect(parseResponse(schema, { id: 'A' }, 'GET issue')).toEqual({
      id: 'A'
    });
  });

  it('throws an ApiError naming the endpoint when the shape is wrong', () => {
    const consoleError = vi
      .spyOn(console, 'error')
      .mockImplementation(() => undefined);

    expect(() => parseResponse(schema, { id: 42 }, 'GET issue')).toThrow(
      ApiError
    );
    expect(() => parseResponse(schema, { id: 42 }, 'GET issue')).toThrow(
      /GET issue/
    );
    expect(consoleError).toHaveBeenCalled();

    consoleError.mockRestore();
  });
});
