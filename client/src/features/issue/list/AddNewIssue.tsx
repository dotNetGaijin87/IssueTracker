import React from 'react';
import { useParams } from 'react-router-dom';
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
import TooltipActionButton from '../../../components/tooltipActionButton/TooltipActionButton';
import FormFieldWrapper from '../../../components/formFieldWrapper/FormFieldWrapper';
import DialogHeader from '../../../components/dialogHeader/DialogHeader';
import HorizontalDivider from '../../../components/horizontalDivider/HorizontalDivider';
import MarkupEditor from '../../../components/markupEditor/MarkupEditor';
import displayError from '../../../helpers/errorHandling/displayError';
import Button from '../../../components/button/Button';
import LoadingButton from '../../../components/loadingButton/LoadingButton';
import IssueTypeSelect from '../../../components/issueType/IssueTypeSelect';
import IssueProgressSelect from '../../../components/issueProgress/IssueProgressSelect';
import IssuePrioritySelect from '../../../components/issuePriority/IssuePrioritySelect';
import AssigneesSetter from '../../../components/assigneesSetter/AssigneesSetter';
import { adapter } from '../../../adapters/adapter';
import { Issue } from '../../../models/issue/issue';
import { IssueType } from '../../../models/issue/issueType';
import { IssueProgress } from '../../../models/issue/issueProgress';
import { IssuePriority } from '../../../models/issue/issuePriority';

function AddNewIssue(): JSX.Element {
  const { projectId } = useParams<{ projectId: string }>();
  const [dialogOpen, setDialogOpen] = React.useState(false);
  const [creatingProject, setCreatingProject] = React.useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
    control,
    setValue
  } = useForm<Issue>({
    defaultValues: {
      id: '',
      projectId: projectId,
      summary: '',
      type: IssueType.Bug,
      progress: IssueProgress.ToDo,
      priority: IssuePriority.Low,
      description: '',
      createdBy: '',
      responsibleBy: [],
      comments: []
    },
    mode: 'all'
  });

  const validateAndSubmitForm = async () => {
    await handleSubmit(async (data) => {
      try {
        setCreatingProject(true);
        await adapter.Issue.create(data);
        toast.success('New issue created');
        setDialogOpen(false);
      } catch (ex: any) {
        displayError(ex, 'Creating issue error');
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
          title={'Add Issue'}
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
            <FormFieldWrapper title="Type">
              <Controller
                control={control}
                name="type"
                render={({ field }) => (
                  <IssueTypeSelect
                    args={field}
                    fullWidth
                    defaultValue={IssueType.Bug}
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
                    args={field}
                    fullWidth
                    defaultValue={IssueProgress.ToDo}
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
                    args={field}
                    fullWidth
                    defaultValue={IssuePriority.Low}
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
              render={({ field: { onChange, onBlur, value } }) => (
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

export default AddNewIssue;
