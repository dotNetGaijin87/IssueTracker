const relativeTime = new Intl.RelativeTimeFormat(undefined, {
  numeric: 'auto'
});

const THRESHOLDS = [
  { limitSeconds: 60, secondsPerUnit: 1, unit: 'second' },
  { limitSeconds: 3600, secondsPerUnit: 60, unit: 'minute' },
  { limitSeconds: 86400, secondsPerUnit: 3600, unit: 'hour' },
  { limitSeconds: 604800, secondsPerUnit: 86400, unit: 'day' }
] as const satisfies readonly {
  limitSeconds: number;
  secondsPerUnit: number;
  unit: Intl.RelativeTimeFormatUnit;
}[];

function parseDateTimeToMessage(date: Date | null | undefined): string {
  if (!date) return '';

  const value = new Date(date);
  if (Number.isNaN(value.getTime())) return '';

  const elapsedSeconds = (Date.now() - value.getTime()) / 1000;

  for (const { limitSeconds, secondsPerUnit, unit } of THRESHOLDS) {
    if (Math.abs(elapsedSeconds) < limitSeconds) {
      return relativeTime.format(
        -Math.round(elapsedSeconds / secondsPerUnit),
        unit
      );
    }
  }

  return value.toLocaleString();
}

export default parseDateTimeToMessage;
