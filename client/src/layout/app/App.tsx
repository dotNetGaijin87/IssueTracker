import { Suspense, useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { Box, CssBaseline, ThemeProvider } from '@mui/material';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import '../../i18n';
import NotFound from '../common/NotFound';
import Settings from '../../features/settings/Page';
import KanbanBoard from '../../features/kanbanBoard/Page';
import UsersPage from '../../features/users/Page';
import { AuthProvider } from '../../authentication/Auth';
import SigningedOutPage from '../../features/login/SigningedOutPage';
import GridCenterFlex from '../common/GridCenterFlex';
import MainContainer from '../common/MainContainer';
import SigningInPage from '../../features/login/SigningPage';
import SigningOutPage from '../../features/login/SigningOutPage';
import AuthorizedPage from '../common/AuthorizedPage';
import ProjectDetailsPage from '../../features/project/details/Page';
import ProjectListPage from '../../features/project/list/Page';
import IssuesListPage from '../../features/issue/list/Page';
import IssueDetailsPage from '../../features/issue/details/Page';
import Header from '../header/Header';
import getTheme from './styles';

function App() {
  const [darkMode, setDarkMode] = useState(true);
  const palleteType = darkMode ? 'dark' : 'light';
  const theme = getTheme(palleteType);

  function handleThemeChange() {
    setDarkMode(!darkMode);
  }

  return (
    <AuthProvider>
      <Suspense fallback={null}>
        <ThemeProvider theme={theme}>
          <CssBaseline />
          <Router>
            <Box display="flex" alignContent="center" alignItems="center">
              <Header />
              <GridCenterFlex>
                <MainContainer>
                  <Routes>
                    <Route
                      path="/kanban"
                      element={
                        <AuthorizedPage>
                          <KanbanBoard />
                        </AuthorizedPage>
                      }
                    />
                    <Route
                      path="/projects"
                      element={
                        <AuthorizedPage>
                          <ProjectListPage />
                        </AuthorizedPage>
                      }
                    />
                    <Route
                      path="/projects/:projectId"
                      element={
                        <AuthorizedPage>
                          <ProjectDetailsPage />
                        </AuthorizedPage>
                      }
                    />
                    <Route
                      path="/projects/:projectId/issues"
                      element={
                        <AuthorizedPage>
                          <IssuesListPage />
                        </AuthorizedPage>
                      }
                    />
                    <Route
                      path="/projects/:projectId/issues/:issueId"
                      element={
                        <AuthorizedPage>
                          <IssueDetailsPage />
                        </AuthorizedPage>
                      }
                    />
                    <Route
                      path="/users"
                      element={
                        <AuthorizedPage>
                          <UsersPage />
                        </AuthorizedPage>
                      }
                    />
                    <Route
                      path="/settings"
                      element={
                        <Settings
                          darkMode={darkMode}
                          handleThemeChange={handleThemeChange}
                        />
                      }
                    />
                    <Route path="signin" element={<SigningInPage />} />
                    <Route path="/signin-callback" element={<KanbanBoard />} />
                    <Route path="signout" element={<SigningOutPage />} />
                    <Route
                      path="/signout-callback"
                      element={<SigningedOutPage />}
                    />
                    <Route path="*" element={<NotFound />} />
                  </Routes>
                </MainContainer>
              </GridCenterFlex>
            </Box>
          </Router>
          <ToastContainer
            theme={palleteType}
            hideProgressBar={true}
            position="bottom-right"
          />
        </ThemeProvider>
      </Suspense>
    </AuthProvider>
  );
}

export default App;
