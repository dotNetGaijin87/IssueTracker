import { useEffect, useState } from 'react';
import {
  Box,
  Divider,
  Grow,
  TableBody,
  TextField,
  Typography
} from '@mui/material';
import { useParams } from 'react-router';
import { useNavigate } from 'react-router-dom';
import LoadingPage from '../../../layout/common/LoadingPage';
import { adapter } from '../../../adapters/adapter';
import { Project, ProjectDefaultValue } from '../../../models/project/project';
import DeleteIcon from '@mui/icons-material/Delete';
import SaveIcon from '@mui/icons-material/Save';
import EditIcon from '@mui/icons-material/Edit';
import CancelIcon from '@mui/icons-material/Cancel';
import BugReportIcon from '@mui/icons-material/BugReport';
import { toast } from 'react-toastify';
import displayError from '../../../helpers/errorHandling/displayError';
import { UserRole } from '../../../models/user/userRole';
import { Controller, useForm } from 'react-hook-form';
import { useAuth } from '../../../authentication/Auth';
import FormFieldWrapper from '../../../components/formFieldWrapper/FormFieldWrapper';
import safelyConvertDateTime from '../../../helpers/time/safelyConvertDateTime';
import MarkupEditor from '../../../components/markupEditor/MarkupEditor';
import Panel from '../../../components/panel/Panel';
import ProjectProgressSelect from '../../../components/projectProgress/ProjectProgressSelect';
import TooltipActionButton from '../../../components/tooltipActionButton/TooltipActionButton';
import ButtonIconWithConfirmationDialog from '../../../components/buttonIconWithConfirmationDialog/ButtonIconWithConfirmationDialog';
import { ProjectProgress } from '../../../models/project/projectProgress';
import ProjectProgressBadge from '../../../components/projectProgress/ProjectProgressBadge';
import VerticalDivider from '../../../components/verticalDivider/VerticalDivider';
import parseDateTimeToMessage from '../../../helpers/time/parseDateTimeToMessage';

function ProjectDetailsPage(): JSX.Element {
  const navigate = useNavigate();
  const { authUser } = useAuth();
  const [loading, setLoading] = useState(false);
  const [editing, setEditing] = useState(false);
  const [updating, setUpdating] = useState(false);
  const { projectId } = useParams<{ projectId: string }>();
  const [project, setProject] = useState<Project>(ProjectDefaultValue);

  const {
    register,
    formState: { errors },
    handleSubmit,
    control,
    getValues,
    reset
  } = useForm<Project>({
    defaultValues: project,
    mode: 'all'
  });

  useEffect(() => {
    const run = async () => {
      try {
        setLoading(true);
        if (projectId) {
          const project = await adapter.Project.get(projectId);
          setProject(project);
          reset(project);
        }
      } catch (ex) {
        displayError(ex, 'Updating data error');
      }
      setLoading(false);
    };

    run();
  }, [projectId]);

  const handleProjectUpdate = async () => {
    setUpdating(true);
    await handleSubmit(async (data) => {
      try {
        let project = await adapter.Project.update({
          id: data.id,
          summary: data.summary,
          progress: data.progress,
          description: data.description
        });
        setProject(project);
        toast.success('Data saved');
      } catch (ex) {
        displayError(ex, 'Saving data error');
      }
      setUpdating(false);
      setEditing(false);
    })();
  };

  const hadleProjectRemoval = async (id: string) => {
    try {
      await adapter.Project.delete(id);
      navigate('/projects');
      toast.success('Project deleted');
    } catch (ex: any) {
      displayError(ex, 'Deleting project error');
    }
  };

  const hadleShwoIssues = () => {
    navigate(`/projects/${project.id}/issues`);
  };

  const canDelete = () =>
    authUser?.role === UserRole.admin ||
    authUser?.name === getValues('createdBy');

  const canModify = () => canDelete();

  const handleEditing = () => {
    setEditing(true);
  };

  const handleCancelEditing = () => {
    setEditing(false);
  };

  if (loading) return <LoadingPage />;

  return (
    <>
      <Box>
        <Box display="flex" justifyContent="space-between" alignItems="center">
          <Box display="flex" alignItems="center">
            <Typography variant="h6">{getValues('id')}</Typography>
            <VerticalDivider />
            {!updating && (
              <ProjectProgressBadge
                value={getValues('progress') as ProjectProgress}
              />
            )}
          </Box>
          <Box display="flex">
            {canDelete() && (
              <ButtonIconWithConfirmationDialog
                hoverOverTitle={'Delete Project'}
                dialogText={'Delete Project?'}
                icon={<DeleteIcon />}
                onConfirm={async () => {
                  await hadleProjectRemoval(getValues('id'));
                }}
              />
            )}

            {canModify() && (
              <>
                {editing ? (
                  <Grow in={true}>
                    <Box display="flex">
                      <VerticalDivider />
                      <TooltipActionButton
                        title={'Save changes'}
                        icon={<SaveIcon />}
                        onClick={handleProjectUpdate}
                      />
                      <TooltipActionButton
                        title={'Cancel changes'}
                        icon={<CancelIcon />}
                        onClick={handleCancelEditing}
                      />
                      <VerticalDivider />
                    </Box>
                  </Grow>
                ) : (
                  <TooltipActionButton
                    title={'Edit Project'}
                    icon={<EditIcon />}
                    onClick={handleEditing}
                  />
                )}
              </>
            )}
            <TooltipActionButton
              title={'Show Issues'}
              icon={<BugReportIcon />}
              onClick={hadleShwoIssues}
            />
          </Box>
        </Box>
        <Divider />
        <Box
          display="flex"
          justifyContent="space-between"
          alignItems="center"
          sx={{ color: 'text.icon', m: 0.5 }}
        >
          <Typography variant="subtitle2">
            {`Created ${parseDateTimeToMessage(
              getValues('creationTime')
            )} by ${getValues('createdBy')}`}
          </Typography>
        </Box>
      </Box>
      <Panel>
        <TableBody>
          <FormFieldWrapper title="Summary">
            <TextField
              fullWidth
              disabled={!canModify() || !editing}
              size="small"
              error
              helperText={
                (errors.summary?.type === 'required' &&
                  'Your input is required') ||
                (errors.summary?.type === 'minLength' &&
                  'Your input is below minimum of 10 characters') ||
                (errors.summary?.type === 'maxLength' &&
                  'Your input exceeds maximum of 100 characters')
              }
              {...register('summary', {
                required: true,
                minLength: 10,
                maxLength: 100
              })}
            />
          </FormFieldWrapper>
          <FormFieldWrapper title="Progress">
            <Controller
              control={control}
              name="progress"
              render={({ field }) => (
                <ProjectProgressSelect
                  args={field}
                  fullWidth
                  defaultValue={ProjectProgress.Open}
                  disabled={!canModify() || !editing}
                />
              )}
            />
          </FormFieldWrapper>
          <FormFieldWrapper title="Creation Time">
            <Controller
              control={control}
              name="creationTime"
              render={({ field: { value } }) => (
                <TextField
                  fullWidth
                  disabled
                  size="small"
                  value={safelyConvertDateTime(value)}
                />
              )}
            />
          </FormFieldWrapper>
          <FormFieldWrapper title="Completion Time">
            <Controller
              control={control}
              name="completionTime"
              render={({ field: { value } }) => (
                <TextField
                  fullWidth
                  disabled
                  size="small"
                  value={safelyConvertDateTime(value)}
                />
              )}
            />
          </FormFieldWrapper>
        </TableBody>
        <Controller
          control={control}
          name="description"
          render={({ field: { onChange, onBlur, value } }) => (
            <MarkupEditor
              title="Description"
              value={value}
              disabled={!canModify() || !editing}
              onBlur={onBlur}
              onChange={onChange}
            />
          )}
        />
      </Panel>
    </>
  );
}

export default ProjectDetailsPage;
