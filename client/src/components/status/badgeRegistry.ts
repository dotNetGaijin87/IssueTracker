import type { ReactNode } from 'react';

export type BadgeVariant = 'chip' | 'text' | 'plain';

export type BadgeSpec = {
  color: string;
  backgroundColor: string;
  icon?: ReactNode;
};

export type BadgeRegistry<T extends string> = Readonly<Record<T, BadgeSpec>>;
