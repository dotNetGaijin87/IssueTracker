import { describe, expect, it } from 'vitest';
import { ApiError, errorMessage, isApiError, toApiError } from './apiError';

describe('toApiError', () => {
  it('passes an existing ApiError through untouched', () => {
    const original = new ApiError('boom', 418);
    expect(toApiError(original, 'fallback')).toBe(original);
  });

  it('prefers the ProblemDetails detail the API returns', () => {
    const axiosLike = {
      message: 'Request failed with status code 400',
      response: { status: 400, data: { title: 'Error', detail: 'Bad summary' } }
    };

    const error = toApiError(axiosLike, 'fallback');
    expect(error.message).toBe('Bad summary');
    expect(error.status).toBe(400);
  });

  it('falls back to the transport message when there is no detail', () => {
    const axiosLike = {
      message: 'Network Error',
      response: { status: 503, data: {} }
    };

    expect(toApiError(axiosLike, 'fallback').message).toBe('Network Error');
  });

  it('handles a plain Error', () => {
    expect(toApiError(new Error('kaboom'), 'fallback').message).toBe('kaboom');
  });

  it.each([undefined, null, '', 0, {}])(
    'uses the fallback for non-error value %p',
    (thrown) => {
      expect(toApiError(thrown, 'fallback').message).toBe('fallback');
    }
  );
});

describe('isApiError', () => {
  it('narrows only genuine ApiError instances', () => {
    expect(isApiError(new ApiError('x'))).toBe(true);
    expect(isApiError(new Error('x'))).toBe(false);
    expect(isApiError('x')).toBe(false);
  });
});

describe('errorMessage', () => {
  it('extracts a displayable message from unknown input', () => {
    expect(errorMessage({ message: 'nope' }, 'fallback')).toBe('nope');
    expect(errorMessage(Symbol('weird'), 'fallback')).toBe('fallback');
  });
});
