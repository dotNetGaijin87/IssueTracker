import React, { useEffect } from 'react';
import { Box, Grow, TextField } from '@mui/material';
import AutorenewOutlinedIcon from '@mui/icons-material/AutorenewOutlined';
import AddNewProject from './AddNewProject';
import { ProjectProgress } from '../../../models/project/projectProgress';
import delayExec from '../../../helpers/delayExec';
import TooltipActionButton from '../../../components/tooltipActionButton/TooltipActionButton';
import Bar from '../../../components/bar/Bar';
import Field from '../../../components/field/Field';
import ProjectProgressSelect from '../../../components/projectProgress/ProjectProgressSelect';
import { useAuth } from '../../../authentication/Auth';
import { UserRole } from '../../../models/user/userRole';

interface Props {
  onSearch: (value: object) => void;
}

function ActionBar({ onSearch }: Props) {
  const { authUser } = useAuth();
  const [initRender, setInitRender] = React.useState(true);
  const [id, setId] = React.useState<string>('');
  const [createdBy, setCreatedBy] = React.useState<string>('');
  const [progress, setProgress] = React.useState<ProjectProgress | undefined>(
    undefined
  );
  const [updateData, setUpdateData] = React.useState(false);

  useEffect(() => {
    if (initRender) {
      setInitRender(false);
      return;
    }
    setUpdateData(false);
    let search: any = {
      progress: progress,
      id: id,
      createdBy: createdBy
    };
    return delayExec(() => onSearch({ ...search }), 1500);
  }, [createdBy, id, updateData, progress]);

  return (
    <Grow in={true}>
      <Box display="flex" alignItems="center" justifyContent="space-between">
        <Bar title="Filter">
          <Field>
            <TextField
              autoFocus
              size="small"
              value={id}
              onChange={(e) => {
                setId(e.target.value);
              }}
              label="Name"
            />
          </Field>

          <Field>
            <TextField
              size="small"
              value={createdBy}
              onChange={(e) => {
                setCreatedBy(e.target.value);
              }}
              label="Created By"
            />
          </Field>
          <Field>
            <ProjectProgressSelect
              withNotApplicable={true}
              defaultValue={ProjectProgress.Unspecified}
              label="Progress"
              onChange={setProgress}
            />
          </Field>
        </Bar>

        <Box display="flex" alignItems="center">
          <TooltipActionButton
            title={'Refresh'}
            icon={<AutorenewOutlinedIcon />}
            onClick={() => setUpdateData(true)}
          />
          {(authUser?.role === UserRole.admin ||
            authUser?.role === UserRole.manager) && <AddNewProject />}
        </Box>
      </Box>
    </Grow>
  );
}

export default ActionBar;
