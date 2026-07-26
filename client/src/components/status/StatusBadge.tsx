import BaseBadge from './BaseBadge';
import type { BadgeRegistry, BadgeVariant } from './badgeRegistry';

export type StatusBadgeProps<T extends string> = {
  value: T | undefined;
  registry: BadgeRegistry<T>;
  variant?: BadgeVariant;
};

export type BoundStatusBadgeProps<T extends string> = Omit<
  StatusBadgeProps<T>,
  'registry'
>;

function StatusBadge<T extends string>({
  value,
  registry,
  variant = 'chip'
}: StatusBadgeProps<T>) {
  if (value === undefined) return null;

  return <BaseBadge label={value} spec={registry[value]} variant={variant} />;
}

export default StatusBadge;
