import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import parseDateTimeToMessage from './parseDateTimeToMessage';

const NOW = new Date('2024-06-01T12:00:00.000Z');

const relativeTime = new Intl.RelativeTimeFormat(undefined, {
  numeric: 'auto'
});

const secondsAgo = (seconds: number) =>
  new Date(NOW.getTime() - seconds * 1000);

describe('parseDateTimeToMessage', () => {
  beforeEach(() => {
    vi.useFakeTimers();
    vi.setSystemTime(NOW);
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  it('returns an empty string for absent dates', () => {
    expect(parseDateTimeToMessage(undefined)).toBe('');
    expect(parseDateTimeToMessage(null)).toBe('');
  });

  it('returns an empty string for an invalid date', () => {
    expect(parseDateTimeToMessage(new Date('nonsense'))).toBe('');
  });

  it('pluralises through Intl instead of the old "1 seconds ago"', () => {
    expect(parseDateTimeToMessage(secondsAgo(1))).toBe(
      relativeTime.format(-1, 'second')
    );
    expect(parseDateTimeToMessage(secondsAgo(30))).toBe(
      relativeTime.format(-30, 'second')
    );
  });

  it('does not round a fresh timestamp up to a whole second', () => {
    expect(parseDateTimeToMessage(secondsAgo(0.2))).toBe(
      relativeTime.format(0, 'second')
    );
  });

  it('steps up through minutes, hours and days', () => {
    expect(parseDateTimeToMessage(secondsAgo(5 * 60))).toBe(
      relativeTime.format(-5, 'minute')
    );
    expect(parseDateTimeToMessage(secondsAgo(3 * 3600))).toBe(
      relativeTime.format(-3, 'hour')
    );
    expect(parseDateTimeToMessage(secondsAgo(2 * 86400))).toBe(
      relativeTime.format(-2, 'day')
    );
  });

  it('switches unit exactly at each threshold', () => {
    expect(parseDateTimeToMessage(secondsAgo(59))).toBe(
      relativeTime.format(-59, 'second')
    );
    expect(parseDateTimeToMessage(secondsAgo(60))).toBe(
      relativeTime.format(-1, 'minute')
    );
  });

  it('falls back to an absolute timestamp beyond a week', () => {
    const old = secondsAgo(30 * 86400);
    expect(parseDateTimeToMessage(old)).toBe(old.toLocaleString());
  });
});
