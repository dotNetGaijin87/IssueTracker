import { describe, expect, it } from 'vitest';
import extractUserId from './extractUserId';
import extractUserRole from './extractUserRole';

describe('extractUserId', () => {
  it('takes the segment after the provider prefix', () => {
    expect(extractUserId('auth0|1a2b3c')).toBe('1a2b3c');
  });

  it('accepts a subject with no prefix', () => {
    expect(extractUserId('1a2b3c')).toBe('1a2b3c');
  });

  it.each([undefined, '', 'auth0|'])(
    'reports %p as absent instead of returning a bogus id',
    (subject) => {
      expect(extractUserId(subject)).toBeUndefined();
    }
  );
});

describe('extractUserRole', () => {
  const claim = 'http://namespace//roles';

  it('reads the first role from the namespaced claim', () => {
    expect(extractUserRole({ [claim]: ['manager'] })).toBe('manager');
  });

  it('returns undefined when the claim is missing', () => {
    expect(extractUserRole({})).toBeUndefined();
    expect(extractUserRole(undefined)).toBeUndefined();
  });

  it('returns undefined for a role the app does not know', () => {
    expect(extractUserRole({ [claim]: ['superuser'] })).toBeUndefined();
  });

  it('returns undefined for an empty role array', () => {
    expect(extractUserRole({ [claim]: [] })).toBeUndefined();
  });
});
