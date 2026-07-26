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
import { adapter, type ProjectListCriteria } from '@/adapters/adapter';
import Pagination from '@/components/pagination/Pagination';
import ProjectProgressBadge from '@/components/projectProgress/ProjectProgressBadge';
import TableContainer from '@/components/tableContainer/TableContainer';
import safelyConvertDateTime from '@/helpers/time/safelyConvertDateTime';
import { useAsyncResource } from '@/helpers/useAsyncResource';
import LoadingPage from '@/layout/common/LoadingPage';
import { emptyPage, type Paginated } from '@/models/pagination';
import type { Project } from '@/models/project/project';
import ActionBar from './ActionBar';

const EMPTY: Paginated<Project> = emptyPage<Project>();

function ProjectListPage() {
  const [criteria, setCriteria] = useState<ProjectListCriteria>({});

  const load = useCallback(() => adapter.Project.list(criteria), [criteria]);

  const { data, loading } = useAsyncResource(
    load,
    EMPTY,
    'Loading projects failed'
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
                    <TableCell>Progress</TableCell>
                    <TableCell>Summary</TableCell>
                    <TableCell>Created by</TableCell>
                    <TableCell>Creation Time</TableCell>
                    <TableCell>Completion Time</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {data.items.map((project) => (
                    <TableRow key={project.id}>
                      <TableCell>
                        <IconButton
                          component={Link}
                          to={`/projects/${project.id}`}
                        >
                          <ArrowRightIcon />
                        </IconButton>
                      </TableCell>
                      <TableCell>{project.id}</TableCell>
                      <TableCell>
                        <ProjectProgressBadge value={project.progress} />
                      </TableCell>
                      <TableCell>{project.summary}</TableCell>
                      <TableCell>{project.createdBy}</TableCell>
                      <TableCell>
                        {safelyConvertDateTime(project.creationTime)}
                      </TableCell>
                      <TableCell>
                        {safelyConvertDateTime(project.completionTime)}
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

export default ProjectListPage;
