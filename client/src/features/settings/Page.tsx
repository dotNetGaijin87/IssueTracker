import { Box, Stack, Switch, Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';

interface Props {
  darkMode: boolean;
  handleThemeChange: () => void;
}

function Settings({ darkMode, handleThemeChange }: Props) {
  const { t } = useTranslation();

  return (
    <Box
      sx={{
        maxWidth: 480,
        mx: 'auto',
        mt: 4,
        p: 3,
        borderRadius: '14px',
        border: '1px solid',
        borderColor: 'divider',
        bgcolor: 'background.paper'
      }}
    >
      <Typography
        sx={{
          color: 'text.secondary',
          fontSize: '0.72rem',
          fontWeight: 700,
          textTransform: 'uppercase',
          letterSpacing: '0.06em',
          mb: 2
        }}
      >
        {t('settings.theme.title')}
      </Typography>
      <Stack direction="row" spacing={1.5} alignItems="center">
        <Typography color="text.secondary">
          {t('settings.theme.light')}
        </Typography>
        <Switch color="primary" checked={darkMode} onChange={handleThemeChange} />
        <Typography>{t('settings.theme.dark')}</Typography>
      </Stack>
    </Box>
  );
}

export default Settings;
