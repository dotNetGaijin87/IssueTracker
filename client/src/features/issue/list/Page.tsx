import { useCallback, useState, type ChangeEvent } from 'react';
import {
  Fade,
  IconButton,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow
} from '@mui/material';
import ArrowRightIcon from '@mui/icons-material/ArrowRight';
import { Link } from 'react-router-dom';
import { adapter, type IssueListCriteria } from '@/adapters/adapter';
import IssuePriorityBadge from '@/components/issuePriority/IssuePriorityBadge';
import IssueProgressBadge from '@/components/issueProgress/IssueProgressBadge';
import IssueTypeBadge from '@/components/issueType/IssueTypeBadge';
import Pagination from '@/components/pagination/Pagination';
import TableContainer from '@/components/tableContainer/TableContainer';
import { useProjectId } from '@/helpers/routing/useRouteId';
import safelyConvertDateTime from '@/helpers/time/safelyConvertDateTime';
import { useAsyncResource } from '@/helpers/useAsyncResource';
import LoadingPage from '@/layout/common/LoadingPage';
import type { Issue } from '@/models/issue/issue';
import { emptyPage, type Paginated } from '@/models/pagination';
import ActionBar from './ActionBar';

const MAX_SUMMARY_LENGTH = 30;

const EMPTY: Paginated<Issue> = emptyPage<Issue>();

function truncate(value: string, maxLength: number): string {
  return value.length > maxLength ? `${value.slice(0, maxLength)}...` : value;
}

function IssuesListPage() {
  const projectId = useProjectId();
  const [criteria, setCriteria] = useState<IssueListCriteria>({});

  const load = useCallback(
    () => adapter.Issue.list({ ...criteria, projectId }),
    [criteria, projectId]
  );

  const { data, loading } = useAsyncResource(
    load,
    EMPTY,
    'Loading issues failed'
  );

  const handlePaginationChange = (
    _event: ChangeEvent<unknown>,
    page: number
  ) => {
    setCriteria((current) => ({ ...current, page }));
  };

  return (
    <>
      <ActionBar onSearch={setCriteria} />
      {loading ? (
        <LoadingPage />
      ) : (
        <>
          <TableContainer>
            <Fade in={true}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Details</TableCell>
                    <TableCell>Name</TableCell>
                    <TableCell>Priority</TableCell>
                    <TableCell>Type</TableCell>
                    <TableCell>Progress</TableCell>
                    <TableCell>Summary</TableCell>
                    <TableCell>Author</TableCell>
                    <TableCell>Creation Time</TableCell>
                    <TableCell>Completion Time</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {data.items.map((issue) => (
                    <TableRow key={issue.id}>
                      <TableCell>
                        <IconButton
                          component={Link}
                          to={`/projects/${issue.projectId}/issues/${issue.id}`}
                        >
                          <ArrowRightIcon />
                        </IconButton>
                      </TableCell>
                      <TableCell>{issue.id}</TableCell>
                      <TableCell>
                        <IssuePriorityBadge value={issue.priority} />
                      </TableCell>
                      <TableCell>
                        <IssueTypeBadge value={issue.type} variant="plain" />
                      </TableCell>
                      <TableCell>
                        <IssueProgressBadge value={issue.progress} />
                      </TableCell>
                      <TableCell>
                        {truncate(issue.summary, MAX_SUMMARY_LENGTH)}
                      </TableCell>
                      <TableCell>{issue.createdBy}</TableCell>
                      <TableCell>
                        {safelyConvertDateTime(issue.creationTime)}
                      </TableCell>
                      <TableCell>
                        {safelyConvertDateTime(issue.completionTime)}
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </Fade>
          </TableContainer>
          <Pagination
            pageCount={data.pageCount}
            page={data.page}
            onChange={handlePaginationChange}
          />
        </>
      )}
    </>
  );
}

export default IssuesListPage;
