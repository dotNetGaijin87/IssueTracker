import React from 'react';
import { Controller, useForm } from 'react-hook-form';
import {
  Box,
  Dialog,
  DialogActions,
  DialogContent,
  Grid,
  TextField
} from '@mui/material';
import AddBoxIcon from '@mui/icons-material/AddBox';
import { toast } from 'react-toastify';
import { useAuth } from '../../../authentication/Auth';
import { adapter } from '../../../adapters/adapter';
import { Project } from '../../../models/project/project';
import { ProjectProgress } from '../../../models/project/projectProgress';
import TooltipActionButton from '../../../components/tooltipActionButton/TooltipActionButton';
import ProjectProgressSelect from '../../../components/projectProgress/ProjectProgressSelect';
import FormFieldWrapper from '../../../components/formFieldWrapper/FormFieldWrapper';
import DialogHeader from '../../../components/dialogHeader/DialogHeader';
import HorizontalDivider from '../../../components/horizontalDivider/HorizontalDivider';
import MarkupEditor from '../../../components/markupEditor/MarkupEditor';
import displayError from '../../../helpers/errorHandling/displayError';
import Button from '../../../components/button/Button';
import LoadingButton from '../../../components/loadingButton/LoadingButton';

function AddNewProject(): JSX.Element {
  const { authUser } = useAuth();
  const [dialogOpen, setDialogOpen] = React.useState(false);
  const [creatingProject, setCreatingProject] = React.useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
    control
  } = useForm<Project>({
    defaultValues: {
      id: '',
      description: '',
      createdBy: authUser?.name,
      creationTime: undefined,
      completionTime: undefined,
      progress: ProjectProgress.Open,
      issues: []
    },
    mode: 'all'
  });

  const validateAndSubmitForm = async () => {
    await handleSubmit(async (data) => {
      try {
        setCreatingProject(true);
        await adapter.Project.create(data);
        toast.success('New project created');
        setDialogOpen(false);
      } catch (ex: any) {
        displayError(ex, 'Creating project error');
      }
      setCreatingProject(false);
    })();
  };

  const handleClose = () => {
    setDialogOpen(false);
  };

  const handleOpen = () => {
    setDialogOpen(true);
  };

  return (
    <>
      <Box>
        <TooltipActionButton
          title={'Add project'}
          icon={<AddBoxIcon />}
          onClick={handleOpen}
        />
      </Box>
      <Grid>
        <Dialog open={dialogOpen} onClose={handleClose} fullWidth>
          <DialogHeader text="Add new project" />
          <DialogContent>
            <FormFieldWrapper title="Id">
              <TextField
                fullWidth
                size="small"
                error
                helperText={
                  (errors.id?.type === 'required' &&
                    'Your input is required') ||
                  (errors.id?.type === 'minLength' &&
                    'Your input is below minimum of 3 characters') ||
                  (errors.id?.type === 'maxLength' &&
                    'Your input exceeds maximum of 50 characters')
                }
                {...register('id', {
                  required: true,
                  minLength: 3,
                  maxLength: 50
                })}
              />
            </FormFieldWrapper>
            <HorizontalDivider />
            <FormFieldWrapper title="Summary">
              <TextField
                fullWidth
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
                  />
                )}
              />
            </FormFieldWrapper>
            <Controller
              control={control}
              name="description"
              render={({
                field: { onChange, onBlur, value, name, ref },
                fieldState: { invalid, isTouched, isDirty, error },
                formState
              }) => (
                <MarkupEditor
                  title="Description"
                  onBlur={onBlur}
                  onChange={onChange}
                  value={value ? value : ''}
                />
              )}
            />
            <DialogActions>
              <Box component="span">
                <Button label="Cancel" onClick={handleClose} />
                <LoadingButton
                  label="Create"
                  loading={creatingProject}
                  onClick={validateAndSubmitForm}
                />
              </Box>
            </DialogActions>
          </DialogContent>
        </Dialog>
      </Grid>
    </>
  );
}

export default AddNewProject;
