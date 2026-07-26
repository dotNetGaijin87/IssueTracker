import { useTranslation } from 'react-i18next';

const ROUTE_KEYS = [
  'home',
  'projects',
  'issues',
  'comments',
  'kanban',
  'settings',
  'signin',
  'signin-callback',
  'signout',
  'signout-callback',
  'users'
] as const;

export type RouteKey = (typeof ROUTE_KEYS)[number];

export type PathToNameMap = Readonly<Record<RouteKey, string>>;

export function usePathToNameMap(): PathToNameMap {
  const { t } = useTranslation();

  return Object.fromEntries(
    ROUTE_KEYS.map((key) => [key, t(`routes.${key}`)])
  ) as PathToNameMap;
}

export function isRouteKey(value: string): value is RouteKey {
  return (ROUTE_KEYS as readonly string[]).includes(value);
}
