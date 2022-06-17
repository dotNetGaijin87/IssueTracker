import { useTranslation } from 'react-i18next';

const PathToNameMap = () => {
  const { t } = useTranslation();

  return {
    home: t('routes.home'),
    projects: t('routes.projects'),
    issues: t('routes.issues'),
    comments: t('routes.comments'),
    kanban: t('routes.kanban'),
    settings: t('routes.settings'),
    signin: t('routes.signin'),
    'signin-callback': t('routes.signin-callback'),
    signout: t('routes.signout'),
    'signout-callback': t('routes.signout-callback'),
    users: t('routes.users')
  };
};

export { PathToNameMap as pathToNameMap };
