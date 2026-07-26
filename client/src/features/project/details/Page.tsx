import { useCallback, useEffect, useState } from 'react';
import { Box, Divider, Grow, TextField, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { Controller, useForm } from 'react-hook-form';
import BugReportIcon from '@mui/icons-material/BugReport';
import CancelIcon from '@mui/icons-material/Cancel';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import { adapter, type UpdateProjectRequest } from '@/adapters/adapter';
import { useAuth } from '@/authentication/Auth';
import ButtonIconWithConfirmationDialog from '@/components/buttonIconWithConfirmationDialog/ButtonIconWithConfirmationDialog';
import FormFieldWrapper from '@/components/formFieldWrapper/FormFieldWrapper';
import MarkupEditor from '@/components/markupEditor/MarkupEditor';
import Panel from '@/components/panel/Panel';
import ProjectProgressBadge from '@/components/projectProgress/ProjectProgressBadge';
import ProjectProgressSelect from '@/components/projectProgress/ProjectProgressSelect';
import TooltipActionButton from '@/components/tooltipActionButton/TooltipActionButton';
import VerticalDivider from '@/components/verticalDivider/VerticalDivider';
import displayError from '@/helpers/errorHandling/displayError';
import { SUMMARY_RULE } from '@/helpers/forms/validationRules';
import { useProjectId } from '@/helpers/routing/useRouteId';
import parseDateTimeToMessage from '@/helpers/time/parseDateTimeToMessage';
import safelyConvertDateTime from '@/helpers/time/safelyConvertDateTime';
import { useAsyncResource } from '@/helpers/useAsyncResource';
import LoadingPage from '@/layout/common/LoadingPage';
import NotFound from '@/layout/common/NotFound';
import { projectCapabilities } from '@/models/access';
import type { Project } from '@/models/project/project';
import { ProjectProgress } from '@/models/project/projectProgress';

type FormValues = Omit<UpdateProjectRequest, 'id'>;

function ProjectDetailsPage() {
  const navigate = useNavigate();
  const { authUser } = useAuth();
  const projectId = useProjectId();
  const [editing, setEditing] = useState(false);
  const [updating, setUpdating] = useState(false);

  const load = useCallback(() => {
    if (projectId === undefined) return Promise.resolve(undefined);
    return adapter.Project.get(projectId);
  }, [projectId]);

  const { data: project, loading } = useAsyncResource<Project | undefined>(
    load,
    undefined,
    'Loading project failed'
  );

  const [current, setCurrent] = useState<Project | undefined>(undefined);

  const { register, formState, handleSubmit, control, reset } =
    useForm<FormValues>({ mode: 'all' });

  useEffect(() => {
    if (project === undefined) return;
    setCurrent(project);
    reset({
      summary: project.summary,
      description: project.description,
      progress: project.progress
    });
  }, [project, reset]);

  const capabilities = projectCapabilities({
    role: authUser?.role,
    isOwner: authUser?.id !== undefined && authUser.id === current?.createdBy
  });

  const readOnly = !capabilities.canModify || !editing;

  const handleProjectUpdate = handleSubmit(async (values) => {
    if (current === undefined) return;

    try {
      setUpdating(true);
      const updated = await adapter.Project.update({
        id: current.id,
        ...values
      });
      setCurrent(updated);
      toast.success('Data saved');
      setEditing(false);
    } catch (error) {
      displayError(error, 'Saving data error');
    } finally {
      setUpdating(false);
    }
  });

  const handleProjectRemoval = async () => {
    if (current === undefined) return;

    try {
      await adapter.Project.delete(current.id);
      navigate('/projects');
      toast.success('Project deleted');
    } catch (error) {
      displayError(error, 'Deleting project error');
    }
  };

  if (loading) return <LoadingPage />;
  if (current === undefined) return <NotFound />;

  return (
    <>
      <Box>
        <Box display="flex" justifyContent="space-between" alignItems="center">
          <Box display="flex" alignItems="center">
            <Typography variant="h6">{current.id}</Typography>
            <VerticalDivider />
            {!updating && <ProjectProgressBadge value={current.progress} />}
          </Box>
          <Box display="flex">
            {capabilities.canDelete && (
              <ButtonIconWithConfirmationDialog
                hoverOverTitle="Delete Project"
                dialogText="Delete Project?"
                icon={<DeleteIcon />}
                onConfirm={handleProjectRemoval}
              />
            )}

            {capabilities.canModify &&
              (editing ? (
                <Grow in={true}>
                  <Box display="flex">
                    <VerticalDivider />
                    <TooltipActionButton
                      title="Save changes"
                      icon={<SaveIcon />}
                      onClick={handleProjectUpdate}
                    />
                    <TooltipActionButton
                      title="Cancel changes"
                      icon={<CancelIcon />}
                      onClick={() => {
                        setEditing(false);
                      }}
                    />
                    <VerticalDivider />
                  </Box>
                </Grow>
              ) : (
                <TooltipActionButton
                  title="Edit Project"
                  icon={<EditIcon />}
                  onClick={() => {
                    setEditing(true);
                  }}
                />
              ))}
            <TooltipActionButton
              title="Show Issues"
              icon={<BugReportIcon />}
              onClick={() => {
                navigate(`/projects/${current.id}/issues`);
              }}
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
            {`Created ${parseDateTimeToMessage(current.creationTime)} by ${current.createdBy}`}
          </Typography>
        </Box>
      </Box>
      <Panel>
        <div style={{ width: '40%' }}>
          <FormFieldWrapper title="Summary" highlighted={editing}>
            <TextField
              fullWidth
              disabled={readOnly}
              size="small"
              error={formState.errors.summary !== undefined}
              helperText={formState.errors.summary?.message}
              {...register('summary', SUMMARY_RULE)}
            />
          </FormFieldWrapper>
          <FormFieldWrapper title="Progress" highlighted={editing}>
            <Controller
              control={control}
              name="progress"
              render={({ field }) => (
                <ProjectProgressSelect
                  fullWidth
                  disabled={readOnly}
                  value={field.value}
                  onBlur={field.onBlur}
                  onChange={(value) => {
                    field.onChange(value ?? ProjectProgress.Open);
                  }}
                />
              )}
            />
          </FormFieldWrapper>
          <FormFieldWrapper title="Creation Time">
            <TextField
              fullWidth
              disabled
              size="small"
              value={safelyConvertDateTime(current.creationTime)}
            />
          </FormFieldWrapper>
          <FormFieldWrapper title="Completion Time">
            <TextField
              fullWidth
              disabled
              size="small"
              value={safelyConvertDateTime(current.completionTime)}
            />
          </FormFieldWrapper>
        </div>
        <Controller
          control={control}
          name="description"
          render={({ field }) => (
            <MarkupEditor
              title="Description"
              value={field.value}
              disabled={readOnly}
              onBlur={field.onBlur}
              onChange={field.onChange}
            />
          )}
        />
      </Panel>
    </>
  );
}

export default ProjectDetailsPage;
