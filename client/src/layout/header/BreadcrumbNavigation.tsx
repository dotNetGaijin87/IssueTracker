import { Typography } from '@mui/material';
import Breadcrumbs from '@mui/material/Breadcrumbs';
import Link, { type LinkProps } from '@mui/material/Link';
import NavigateNextIcon from '@mui/icons-material/NavigateNext';
import { Link as RouterLink, useLocation } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { isRouteKey, usePathToNameMap } from '@/consts/pathToNameMap';

type LinkRouterProps = LinkProps<typeof RouterLink> & {
  to: string;
  replace?: boolean;
};

const LinkRouter = (props: LinkRouterProps) => (
  <Link {...props} component={RouterLink} />
);

const BreadcrumbNavigation = () => {
  const location = useLocation();
  const { t } = useTranslation();
  const names = usePathToNameMap();

  const segments = location.pathname.split('/').filter(Boolean);
  const label = (segment: string) =>
    isRouteKey(segment) ? names[segment] : segment;

  return (
    <Breadcrumbs
      aria-label="breadcrumb"
      separator={<NavigateNextIcon fontSize="small" />}
    >
      <LinkRouter underline="hover" color="inherit" to="/">
        {t('routes.home')}
      </LinkRouter>
      {segments.map((segment, index) => {
        const to = `/${segments.slice(0, index + 1).join('/')}`;
        const isLast = index === segments.length - 1;

        return isLast ? (
          <Typography color="text.primary" key={to}>
            {label(segment)}
          </Typography>
        ) : (
          <LinkRouter underline="hover" color="inherit" to={to} key={to}>
            {label(segment)}
          </LinkRouter>
        );
      })}
    </Breadcrumbs>
  );
};

export default BreadcrumbNavigation;
