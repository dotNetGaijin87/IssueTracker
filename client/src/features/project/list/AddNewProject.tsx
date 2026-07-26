import { useState } from 'react';
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
import { adapter, type CreateProjectRequest } from '@/adapters/adapter';
import Button from '@/components/button/Button';
import DialogHeader from '@/components/dialogHeader/DialogHeader';
import FormFieldWrapper from '@/components/formFieldWrapper/FormFieldWrapper';
import HorizontalDivider from '@/components/horizontalDivider/HorizontalDivider';
import LoadingButton from '@/components/loadingButton/LoadingButton';
import MarkupEditor from '@/components/markupEditor/MarkupEditor';
import ProjectProgressSelect from '@/components/projectProgress/ProjectProgressSelect';
import TooltipActionButton from '@/components/tooltipActionButton/TooltipActionButton';
import displayError from '@/helpers/errorHandling/displayError';
import { NAME_RULE, SUMMARY_RULE } from '@/helpers/forms/validationRules';
import { ProjectProgress } from '@/models/project/projectProgress';

const DEFAULT_VALUES: CreateProjectRequest = {
  id: '',
  summary: '',
  description: '',
  progress: ProjectProgress.Open
};

function AddNewProject() {
  const [dialogOpen, setDialogOpen] = useState(false);
  const [creating, setCreating] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
    control,
    reset
  } = useForm<CreateProjectRequest>({
    defaultValues: DEFAULT_VALUES,
    mode: 'all'
  });

  const validateAndSubmitForm = handleSubmit(async (values) => {
    try {
      setCreating(true);
      await adapter.Project.create(values);
      toast.success('New project created');
      reset(DEFAULT_VALUES);
      setDialogOpen(false);
    } catch (error) {
      displayError(error, 'Creating project error');
    } finally {
      setCreating(false);
    }
  });

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
          title="Add project"
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
                error={errors.id !== undefined}
                helperText={errors.id?.message}
                {...register('id', NAME_RULE)}
              />
            </FormFieldWrapper>
            <HorizontalDivider />
            <FormFieldWrapper title="Summary">
              <TextField
                fullWidth
                size="small"
                error={errors.summary !== undefined}
                helperText={errors.summary?.message}
                {...register('summary', SUMMARY_RULE)}
              />
            </FormFieldWrapper>
            <FormFieldWrapper title="Progress">
              <Controller
                control={control}
                name="progress"
                render={({ field }) => (
                  <ProjectProgressSelect
                    fullWidth
                    value={field.value}
                    onBlur={field.onBlur}
                    onChange={(value) => {
                      field.onChange(value ?? ProjectProgress.Open);
                    }}
                  />
                )}
              />
            </FormFieldWrapper>
            <Controller
              control={control}
              name="description"
              render={({ field }) => (
                <MarkupEditor
                  title="Description"
                  value={field.value}
                  onBlur={field.onBlur}
                  onChange={field.onChange}
                />
              )}
            />
            <DialogActions>
              <Box component="span">
                <Button label="Cancel" onClick={handleClose} />
                <LoadingButton
                  label="Create"
                  loading={creating}
                  onClick={() => {
                    void validateAndSubmitForm();
                  }}
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
