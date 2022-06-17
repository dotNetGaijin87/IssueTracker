import {
  Divider,
  List,
  ListItem,
  Stack,
  Switch,
  Typography
} from '@mui/material';
import { useTranslation } from 'react-i18next';

interface Props {
  darkMode: boolean;
  handleThemeChange: () => void;
}

function Settings({ darkMode, handleThemeChange }: Props) {
  const { t } = useTranslation();

  return (
    <List>
      <ListItem sx={{ mt: 2 }}>
        <Typography variant="h6">{t('settings.theme.title')}</Typography>
      </ListItem>
      <Divider component="li" sx={{ mt: 1, mb: 2 }} />
      <ListItem>
        <Stack direction="row" spacing={1} alignItems="center">
          <Typography>{t('settings.theme.light')}</Typography>
          <Switch
            color="secondary"
            checked={darkMode}
            onChange={handleThemeChange}
          />
          <Typography>{t('settings.theme.dark')}</Typography>
        </Stack>
      </ListItem>
    </List>
  );
}

export default Settings;
