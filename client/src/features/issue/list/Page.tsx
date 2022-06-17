import React, { useEffect, useState } from 'react';
import {
  Fade,
  IconButton,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow
} from '@mui/material';
import { adapter } from '../../../adapters/adapter';
import LoadingPage from '../../../layout/common/LoadingPage';
import { Issue } from '../../../models/issue/issue';
import TableContainer from '../../../components/tableContainer/TableContainer';
import ArrowRightIcon from '@mui/icons-material/ArrowRight';
import { Link, useParams } from 'react-router-dom';
import safelyConvertDateTime from '../../../helpers/time/safelyConvertDateTime';
import Pagination from '../../../components/pagination/Pagination';
import IssueTypeBadge from '../../../components/issueType/IssueTypeBadge';
import IssueProgressBadge from '../../../components/issueProgress/IssueProgressBadge';
import IssuePriorityBadge from '../../../components/issuePriority/IssuePriorityBadge';
import ActionBar, { IssueListSearchCriteria } from './ActionBar';

interface IssueList {
  issues: Issue[];
  pageCount: number;
  page: number;
}

function IssuesListPage() {
  const { projectId } = useParams<{ projectId: string }>();
  const [lodaing, setLoading] = React.useState(false);
  const [searchCriteria, setSearchCriteria] = useState<any>();
  const [state, setState] = useState<IssueList>({
    issues: [],
    pageCount: 0,
    page: 1
  });

  useEffect(() => {
    const run = async () => {
      try {
        setLoading(true);
        let rsp = await adapter.Issue.list({
          ...searchCriteria,
          projectId: projectId
        });
        setState(rsp);
      } catch (ex) {}
      setLoading(false);
    };

    run();
  }, [searchCriteria]);

  const handlePaginationChange = (
    event: React.ChangeEvent<unknown>,
    page: number
  ) => {
    setSearchCriteria({ ...searchCriteria, page: page });
  };

  const handleSearchReq = (value: IssueListSearchCriteria) => {
    setSearchCriteria(value);
  };

  return (
    <>
      <ActionBar onSearch={handleSearchReq} />
      {lodaing ? (
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
                  {state.issues?.map((issue: Issue) => (
                    <TableRow key={issue.id}>
                      <TableCell>
                        <IconButton
                          component={Link}
                          to={`/projects/${projectId}/issues/${issue.id}`}
                        >
                          <ArrowRightIcon />
                        </IconButton>
                      </TableCell>
                      <TableCell>{issue.id}</TableCell>
                      <TableCell>
                        <IssueTypeBadge value={issue.type} unstyled={true} />
                      </TableCell>
                      <TableCell>
                        <IssuePriorityBadge value={issue.priority} />
                      </TableCell>
                      <TableCell>
                        <IssueProgressBadge value={issue.progress} />
                      </TableCell>
                      <TableCell>
                        {issue.summary.length > 30
                          ? issue.summary.substring(0, 30) + '...'
                          : issue.summary}
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
            pageCount={state.pageCount}
            page={state.page}
            onChange={handlePaginationChange}
          />
        </>
      )}
    </>
  );
}

export default IssuesListPage;
