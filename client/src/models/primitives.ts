import { z } from 'zod';

/** Timestamps arrive as ISO strings; absent ones arrive as `null`, `''` or omitted. */
export const optionalDate = z
  .union([z.string(), z.number(), z.date()])
  .nullish()
  .transform((value) => {
    if (value === null || value === undefined || value === '') return undefined;
    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? undefined : date;
  });

export const requiredDate = z.coerce.date();

export const trimmedString = z.string().trim();
