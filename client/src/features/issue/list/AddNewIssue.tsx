import { useState } from 'react';
import { toast } from 'react-toastify';
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
import { adapter, type CreateIssueRequest } from '@/adapters/adapter';
import AssigneesSetter from '@/components/assigneesSetter/AssigneesSetter';
import Button from '@/components/button/Button';
import DialogHeader from '@/components/dialogHeader/DialogHeader';
import FormFieldWrapper from '@/components/formFieldWrapper/FormFieldWrapper';
import HorizontalDivider from '@/components/horizontalDivider/HorizontalDivider';
import IssuePrioritySelect from '@/components/issuePriority/IssuePrioritySelect';
import IssueProgressSelect from '@/components/issueProgress/IssueProgressSelect';
import IssueTypeSelect from '@/components/issueType/IssueTypeSelect';
import LoadingButton from '@/components/loadingButton/LoadingButton';
import MarkupEditor from '@/components/markupEditor/MarkupEditor';
import TooltipActionButton from '@/components/tooltipActionButton/TooltipActionButton';
import displayError from '@/helpers/errorHandling/displayError';
import { NAME_RULE, SUMMARY_RULE } from '@/helpers/forms/validationRules';
import { useProjectId } from '@/helpers/routing/useRouteId';
import { IssuePriority } from '@/models/issue/issuePriority';
import { IssueProgress } from '@/models/issue/issueProgress';
import { IssueType } from '@/models/issue/issueType';

type FormValues = Omit<CreateIssueRequest, 'projectId'>;

const DEFAULT_VALUES: FormValues = {
  id: '',
  summary: '',
  description: '',
  type: IssueType.Bug,
  progress: IssueProgress.ToDo,
  priority: IssuePriority.Low,
  responsibleBy: []
};

function AddNewIssue() {
  const projectId = useProjectId();
  const [dialogOpen, setDialogOpen] = useState(false);
  const [creating, setCreating] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
    control,
    setValue,
    reset
  } = useForm<FormValues>({ defaultValues: DEFAULT_VALUES, mode: 'all' });

  const validateAndSubmitForm = handleSubmit(async (values) => {
    if (projectId === undefined) return;

    try {
      setCreating(true);
      await adapter.Issue.create({ ...values, projectId });
      toast.success('New issue created');
      reset(DEFAULT_VALUES);
      setDialogOpen(false);
    } catch (error) {
      displayError(error, 'Creating issue error');
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
          title="Add Issue"
          icon={<AddBoxIcon />}
          onClick={handleOpen}
        />
      </Box>
      <Grid>
        <Dialog open={dialogOpen} onClose={handleClose} fullWidth>
          <DialogHeader text="Add new issue" />
          <DialogContent>
            <FormFieldWrapper title="Name">
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
            <FormFieldWrapper title="Type">
              <Controller
                control={control}
                name="type"
                render={({ field }) => (
                  <IssueTypeSelect
                    fullWidth
                    value={field.value}
                    onBlur={field.onBlur}
                    onChange={(value) => {
                      field.onChange(value ?? IssueType.Bug);
                    }}
                  />
                )}
              />
            </FormFieldWrapper>
            <FormFieldWrapper title="Progress">
              <Controller
                control={control}
                name="progress"
                render={({ field }) => (
                  <IssueProgressSelect
                    fullWidth
                    value={field.value}
                    onBlur={field.onBlur}
                    onChange={(value) => {
                      field.onChange(value ?? IssueProgress.ToDo);
                    }}
                  />
                )}
              />
            </FormFieldWrapper>
            <FormFieldWrapper title="Priority">
              <Controller
                control={control}
                name="priority"
                render={({ field }) => (
                  <IssuePrioritySelect
                    fullWidth
                    value={field.value}
                    onBlur={field.onBlur}
                    onChange={(value) => {
                      field.onChange(value ?? IssuePriority.Low);
                    }}
                  />
                )}
              />
            </FormFieldWrapper>
            <FormFieldWrapper title="Assignees">
              <AssigneesSetter
                onChange={(responsibleBy) => {
                  setValue('responsibleBy', responsibleBy);
                }}
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

export default AddNewIssue;
