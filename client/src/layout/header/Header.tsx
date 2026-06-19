import { AppBar, Box, Drawer, Toolbar, Typography } from '@mui/material';
import TooltipNavButtonBase from '@/components/toolTipNavButton/TooltipNavButtonBase';
import ViewKanbanIcon from '@mui/icons-material/ViewKanban';
import GroupIcon from '@mui/icons-material/Group';
import SettingsIcon from '@mui/icons-material/Settings';
import LoginIcon from '@mui/icons-material/Login';
import LogoutIcon from '@mui/icons-material/Logout';
import { useAuth } from '@/authentication/Auth';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import AssignmentIcon from '@mui/icons-material/Assignment';
import BreadcrumbNavigation from './BreadcrumbNavigation';

const drawerWidth = 50;

function Header() {
  const { isAuthenticated, authUser, authInProgress } = useAuth();

  return (
    <>
      <AppBar
        elevation={0}
        sx={{
          bgcolor: 'background.default',
          borderBottom: '1px solid',
          borderColor: 'divider',
          display: 'flex',
          alignItems: 'center',
          flexDirection: 'row',
          justifyContent: 'space-between',
          height: '50px'
        }}
      >
        <Box
          sx={{
            ml: '100px'
          }}
        >
          <BreadcrumbNavigation />
        </Box>
        <Box
          sx={{
            display: 'flex',
            alignItems: 'center',
            flexDirection: 'row'
          }}
        >
          {authUser?.name && (
            <Box
              sx={{
                display: 'flex',
                alignItems: 'center',
                gap: 0.75,
                mr: 2,
                px: 1.25,
                py: 0.5,
                borderRadius: 2,
                border: '1px solid',
                borderColor: 'divider',
                bgcolor: 'background.paper'
              }}
            >
              <AccountCircleIcon sx={{ color: 'primary.main', fontSize: 20 }} />
              <Typography
                sx={{ color: 'text.primary', fontSize: '0.85rem', fontWeight: 500 }}
              >
                {authUser?.name}
              </Typography>
            </Box>
          )}
        </Box>
      </AppBar>
      <Drawer
        variant="permanent"
        anchor="left"
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: drawerWidth,
            boxSizing: 'border-box',
            borderRight: '1px solid',
            borderColor: 'divider'
          }
        }}
      >
        <Toolbar
          sx={{
            display: 'flex',
            alignItems: 'center',
            flexDirection: 'column',
            justifyContent: 'space-between',
            height: '100%'
          }}
        >
          <Box>
            {!authInProgress && isAuthenticated && (
              <>
                <TooltipNavButtonBase
                  icon={<ViewKanbanIcon />}
                  title="Kanban Board"
                  routeTo="/kanban"
                />
                <TooltipNavButtonBase
                  icon={<AssignmentIcon />}
                  title="Projects"
                  routeTo="/projects"
                />
                <TooltipNavButtonBase
                  icon={<GroupIcon />}
                  title="Users"
                  routeTo="/users"
                />
              </>
            )}
          </Box>
          <Box
            sx={{
              mb: 1
            }}
          >
            <TooltipNavButtonBase
              icon={<SettingsIcon />}
              title="Settings"
              routeTo="/settings"
            />
            <div>
              {isAuthenticated || authInProgress ? (
                <TooltipNavButtonBase
                  icon={<LogoutIcon />}
                  title="Sign out"
                  routeTo="/signout"
                />
              ) : (
                <TooltipNavButtonBase
                  icon={<LoginIcon />}
                  title="Sign in"
                  routeTo="/signin"
                />
              )}
            </div>
          </Box>
        </Toolbar>
      </Drawer>
    </>
  );
}

export default Header;
