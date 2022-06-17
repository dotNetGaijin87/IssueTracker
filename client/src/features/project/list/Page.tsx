import { Fragment, useEffect, useState } from 'react';
import ActionBar from './ActionBar';
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
import { Project } from '../../../models/project/project';
import ArrowRightIcon from '@mui/icons-material/ArrowRight';
import { Link } from 'react-router-dom';
import safelyConvertDateTime from '../../../helpers/time/safelyConvertDateTime';
import TableContainer from '../../../components/tableContainer/TableContainer';
import ProjectProgressBadge from '../../../components/projectProgress/ProjectProgressBadge';
import Pagination from '../../../components/pagination/Pagination';

interface ProjectList {
  projects: Project[];
  pageCount: number;
  page: number;
}

function ProjectListPage() {
  const [lodaing, setLoading] = useState(false);
  const [searchCriteria, setSearchCriteria] = useState<any>();
  const [projectList, setProjectList] = useState<ProjectList>({
    projects: [],
    pageCount: 0,
    page: 1
  });

  useEffect(() => {
    const run = async () => {
      try {
        setLoading(true);
        let projectList = (await adapter.Project.list(
          searchCriteria
        )) as ProjectList;
        setProjectList(projectList);
      } catch (ex) {}
      setLoading(false);
    };

    run();
  }, [searchCriteria]);

  const handleSearchReq = (value: object) => {
    setSearchCriteria(value);
  };
  const handlePaginationChange = (event: any, page: number) => {
    setSearchCriteria({ ...searchCriteria, page: page });
  };

  return (
    <Fragment>
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
                    <TableCell>Progress</TableCell>
                    <TableCell>Summary</TableCell>
                    <TableCell>Created by</TableCell>
                    <TableCell>Creation Time</TableCell>
                    <TableCell>Completion Time</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {projectList.projects?.map((project: Project) => (
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
            pageCount={projectList.pageCount}
            page={projectList.page}
            onChange={handlePaginationChange}
          />
        </>
      )}
    </Fragment>
  );
}

export default ProjectListPage;
