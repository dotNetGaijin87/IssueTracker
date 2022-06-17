import { Typography } from '@mui/material';
import Link, { LinkProps } from '@mui/material/Link';
import Breadcrumbs from '@mui/material/Breadcrumbs';
import NavigateNextIcon from '@mui/icons-material/NavigateNext';
import { Link as RouterLink, useLocation } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { pathToNameMap } from '../../consts/pathToNameMap';

interface LinkRouterProps extends LinkProps {
  to: string;
  replace?: boolean;
}

const LinkRouter = (props: LinkRouterProps) => (
  <Link {...props} component={RouterLink as any} />
);

const mapValues = (routes: string, map: any) => {
  const clone: any = routes.split('/');
  clone.forEach((k: string, index: number) => {
    if (map[k]) {
      clone[index] = map[k];
    }
  });
  return clone;
};

const BreadcrumbNavigation = () => {
  const location = useLocation();
  const { t } = useTranslation();
  const pathnames = location.pathname.split('/').filter((x) => x);

  const map = pathToNameMap();

  return (
    <Breadcrumbs
      aria-label="breadcrumb"
      separator={<NavigateNextIcon fontSize="small" />}
    >
      <LinkRouter underline="hover" color="inherit" to="/">
        {t('routes.home')}
      </LinkRouter>
      {pathnames.map((value, index) => {
        const last = index === pathnames.length - 1;
        const to = `/${pathnames.slice(0, index + 1).join('/')}`;

        return last ? (
          <Typography color="text.primary" key={to}>
            {mapValues(value, map)}
          </Typography>
        ) : (
          <LinkRouter underline="hover" color="inherit" to={to} key={to}>
            {mapValues(value, map)}
          </LinkRouter>
        );
      })}
    </Breadcrumbs>
  );
};

export default BreadcrumbNavigation;
