import type { ReactNode } from 'react';

export type BadgeVariant = 'chip' | 'text' | 'plain';

export type BadgeSpec = {
  color: string;
  backgroundColor: string;
  icon?: ReactNode;
};

/**
 * A total map from a status union to its presentation. Adding a member to the
 * union turns every incomplete registry into a compile error.
 */
export type BadgeRegistry<T extends string> = Readonly<Record<T, BadgeSpec>>;
