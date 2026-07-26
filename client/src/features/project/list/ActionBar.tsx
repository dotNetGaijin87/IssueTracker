import { useEffect, useRef, useState } from 'react';
import { Box, Grow, TextField } from '@mui/material';
import AutorenewOutlinedIcon from '@mui/icons-material/AutorenewOutlined';
import type { ProjectListCriteria } from '@/adapters/adapter';
import { useAuth } from '@/authentication/Auth';
import Bar from '@/components/bar/Bar';
import Field from '@/components/field/Field';
import ProjectProgressSelect from '@/components/projectProgress/ProjectProgressSelect';
import TooltipActionButton from '@/components/tooltipActionButton/TooltipActionButton';
import delayExec from '@/helpers/delayExec';
import { isPrivilegedRole } from '@/models/access';
import type { ProjectProgress } from '@/models/project/projectProgress';
import AddNewProject from './AddNewProject';

const SEARCH_DEBOUNCE_MS = 1500;

interface Props {
  onSearch: (criteria: ProjectListCriteria) => void;
}

function ActionBar({ onSearch }: Props) {
  const { authUser } = useAuth();
  const [id, setId] = useState('');
  const [createdBy, setCreatedBy] = useState('');
  const [progress, setProgress] = useState<ProjectProgress | undefined>(
    undefined
  );
  const [refreshToken, setRefreshToken] = useState(0);

  const isInitialRender = useRef(true);
  const onSearchRef = useRef(onSearch);
  onSearchRef.current = onSearch;

  useEffect(() => {
    if (isInitialRender.current) {
      isInitialRender.current = false;
      return;
    }

    return delayExec(() => {
      onSearchRef.current({ id, createdBy, progress });
    }, SEARCH_DEBOUNCE_MS);
  }, [id, createdBy, progress, refreshToken]);

  return (
    <Grow in={true}>
      <Box display="flex" alignItems="center" justifyContent="space-between">
        <Bar title="Filter">
          <Field>
            <TextField
              autoFocus
              size="small"
              value={id}
              onChange={(event) => {
                setId(event.target.value);
              }}
              label="Name"
            />
          </Field>
          <Field>
            <TextField
              size="small"
              value={createdBy}
              onChange={(event) => {
                setCreatedBy(event.target.value);
              }}
              label="Created By"
            />
          </Field>
          <Field>
            <ProjectProgressSelect
              includeAny
              label="Progress"
              value={progress}
              onChange={setProgress}
            />
          </Field>
        </Bar>

        <Box display="flex" alignItems="center">
          <TooltipActionButton
            title="Refresh"
            icon={<AutorenewOutlinedIcon />}
            onClick={() => {
              setRefreshToken((token) => token + 1);
            }}
          />
          {isPrivilegedRole(authUser?.role) && <AddNewProject />}
        </Box>
      </Box>
    </Grow>
  );
}

export default ActionBar;
