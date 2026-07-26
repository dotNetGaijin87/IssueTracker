import { useCallback, useState, type SyntheticEvent } from 'react';
import { TabContext } from '@mui/lab';
import { Tab } from '@mui/material';
import { adapter } from '@/adapters/adapter';
import { useAuth } from '@/authentication/Auth';
import TabList from '@/components/tabList/TabList';
import TabPanel from '@/components/tabPanel/TabPanel';
import { useIssueId } from '@/helpers/routing/useRouteId';
import { useAsyncResource } from '@/helpers/useAsyncResource';
import LoadingPage from '@/layout/common/LoadingPage';
import NotFound from '@/layout/common/NotFound';
import { issueCapabilities } from '@/models/access';
import type { Issue } from '@/models/issue/issue';
import Assignees from './Assignees';
import Comments from './Comments';
import Summary from './Summary';

function IssueDetailsPage() {
  const { authUser } = useAuth();
  const issueId = useIssueId();
  const [tabId, setTabId] = useState('summary');

  const load = useCallback(() => {
    if (issueId === undefined) return Promise.resolve(undefined);
    return adapter.Issue.get(issueId);
  }, [issueId]);

  const { data: issue, loading } = useAsyncResource<Issue | undefined>(
    load,
    undefined,
    'Getting data error'
  );

  const handleTabChange = (_event: SyntheticEvent, value: string) => {
    setTabId(value);
  };

  if (loading) return <LoadingPage />;
  if (issue === undefined) return <NotFound />;

  const capabilities = issueCapabilities({
    permission: issue.permission?.issuePermission,
    role: authUser?.role,
    isOwner: authUser?.id !== undefined && authUser.id === issue.createdBy
  });

  return (
    <TabContext value={tabId}>
      <TabList onChange={handleTabChange}>
        <Tab value="summary" label="Summary" />
        <Tab value="comments" label="Comments" />
        <Tab value="assignees" label="Assignees" />
      </TabList>
      <TabPanel value="summary">
        <Summary issue={issue} capabilities={capabilities} />
      </TabPanel>
      <TabPanel value="comments">
        <Comments capabilities={capabilities} />
      </TabPanel>
      <TabPanel value="assignees">
        <Assignees issue={issue} capabilities={capabilities} />
      </TabPanel>
    </TabContext>
  );
}

export default IssueDetailsPage;
