import { useEffect, useState } from 'react';
import { useParams } from 'react-router';
import { TabContext } from '@mui/lab';
import { Tab } from '@mui/material';
import LoadingPage from '../../../layout/common/LoadingPage';
import { Issue, IssueDefaultValue } from '../../../models/issue/issue';
import Comments from './Comments';
import Summary from './Summary';
import TabPanel from '../../../components/tabPanel/TabPanel';
import TabList from '../../../components/tabList/TabList';
import { adapter } from '../../../adapters/adapter';
import { IssuePermission } from '../../../models/issue/issuePermission';
import { useAuth } from '../../../authentication/Auth';
import { UserRole } from '../../../models/user/userRole';
import Assignees from './Assignees';
import displayError from '../../../helpers/errorHandling/displayError';
import { Permission } from '../../../models/permission/permission';

function IssueDetailsPage(): JSX.Element {
  const { authUser } = useAuth();
  const { issueId } = useParams<{ issueId: string }>();
  const [issue, setIssue] = useState<Issue>(IssueDefaultValue);
  const [loading, setLoading] = useState(false);
  const [tabId, setTabId] = useState('summary');

  const [permission, setPermission] = useState<Permission>();

  useEffect(() => {
    const run = async () => {
      try {
        setLoading(true);
        if (issueId) {
          const issue = await adapter.Issue.get(issueId);
          setIssue(issue);
          setPermission(issue.permission);
        }
      } catch (ex) {
        displayError(ex, 'Getting data error');
      }
      setLoading(false);
    };

    run();
  }, [issueId]);

  const canDelete = () =>
    permission?.issuePermission === IssuePermission.CanDelete ||
    authUser?.role === UserRole.admin ||
    authUser?.role === UserRole.manager ||
    authUser?.name === issue.createdBy;

  const canModify = () =>
    permission?.issuePermission === IssuePermission.CanDelete ||
    permission?.issuePermission === IssuePermission.CanModify ||
    authUser?.role === UserRole.admin ||
    authUser?.role === UserRole.manager;

  const permissions = [
    canDelete() ? IssuePermission.CanDelete : IssuePermission.CanSee,
    canModify() ? IssuePermission.CanModify : IssuePermission.CanSee
  ];

  if (loading) return <LoadingPage />;

  return (
    <TabContext value={tabId}>
      <TabList
        onChange={(event: any, newValue: string) => {
          setTabId(newValue);
        }}
      >
        <Tab value="summary" label="Summary" />
        <Tab value="comments" label="Comments" />
        <Tab value="assignees" label="Assignees" />
      </TabList>
      <TabPanel value={'summary'}>
        <Summary issue={issue} initialPermission={permission} />
      </TabPanel>
      <TabPanel value={'comments'}>
        <Comments permissions={permissions} />
      </TabPanel>
      <TabPanel value={'assignees'}>
        <Assignees issue={issue} permissions={permissions} />
      </TabPanel>
    </TabContext>
  );
}

export default IssueDetailsPage;
