import { describe, expect, it } from 'vitest';
import getFieldMask from './getFieldMask';

describe('getFieldMask', () => {
  it('lists top-level keys', () => {
    expect(getFieldMask({ id: 'A-1', summary: 'text' })).toEqual([
      'id',
      'summary'
    ]);
  });

  it('skips undefined values but keeps null ones', () => {
    expect(
      getFieldMask({ id: 'A-1', summary: undefined, completedOn: null })
    ).toEqual(['id', 'completedOn']);
  });

  it('flattens nested objects into dotted paths', () => {
    expect(getFieldMask({ owner: { id: 'u1', email: 'e' } })).toEqual([
      'owner.id',
      'owner.email'
    ]);
  });

  it('treats arrays as leaves rather than emitting index paths', () => {
    expect(getFieldMask({ responsibleBy: ['a', 'b'] })).toEqual([
      'responsibleBy'
    ]);
  });

  it('treats dates as leaves instead of dropping them', () => {
    expect(getFieldMask({ creationTime: new Date('2024-01-01') })).toEqual([
      'creationTime'
    ]);
  });

  it('returns an empty mask for an empty object', () => {
    expect(getFieldMask({})).toEqual([]);
  });
});
