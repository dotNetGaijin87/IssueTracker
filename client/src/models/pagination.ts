import { z } from 'zod';

export type Paginated<T> = {
  items: T[];
  page: number;
  pageCount: number;
};

export function emptyPage<T>(): Paginated<T> {
  return { items: [], page: 1, pageCount: 0 };
}

/**
 * The API wraps collections under a resource-specific key
 * (`issues`, `projects`, `users`, `comments`); this normalises them to `items`.
 */
export function paginatedSchema<T>(itemsKey: string, item: z.ZodType<T>) {
  return z
    .looseObject({
      page: z.coerce.number().int().nonnegative().catch(1),
      pageCount: z.coerce.number().int().nonnegative().catch(0)
    })
    .transform(
      (raw): Paginated<T> => ({
        items: z
          .array(item)
          .catch([])
          .parse(raw[itemsKey] ?? []),
        page: raw.page,
        pageCount: raw.pageCount
      })
    );
}
