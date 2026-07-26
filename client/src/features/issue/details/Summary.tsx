import { useState } from 'react';
import { Box, Divider, Grow, TextField, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { Controller, useForm } from 'react-hook-form';
import BookmarkAddIcon from '@mui/icons-material/BookmarkAdd';
import BookmarkRemoveIcon from '@mui/icons-material/BookmarkRemove';
import CancelIcon from '@mui/icons-material/Cancel';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import { adapter, type UpdateIssueRequest } from '@/adapters/adapter';
import { useAuth } from '@/authentication/Auth';
import ButtonIconWithConfirmationDialog from '@/components/buttonIconWithConfirmationDialog/ButtonIconWithConfirmationDialog';
import FormFieldWrapper from '@/components/formFieldWrapper/FormFieldWrapper';
import IssuePriorityBadge from '@/components/issuePriority/IssuePriorityBadge';
import IssuePrioritySelect from '@/components/issuePriority/IssuePrioritySelect';
import IssueProgressBadge from '@/components/issueProgress/IssueProgressBadge';
import IssueProgressSelect from '@/components/issueProgress/IssueProgressSelect';
import IssueTypeSelect from '@/components/issueType/IssueTypeSelect';
import MarkupEditor from '@/components/markupEditor/MarkupEditor';
import Panel from '@/components/panel/Panel';
import TooltipActionButton from '@/components/tooltipActionButton/TooltipActionButton';
import VerticalDivider from '@/components/verticalDivider/VerticalDivider';
import displayError from '@/helpers/errorHandling/displayError';
import { SUMMARY_RULE } from '@/helpers/forms/validationRules';
import parseDateTimeToMessage from '@/helpers/time/parseDateTimeToMessage';
import safelyConvertDateTime from '@/helpers/time/safelyConvertDateTime';
import type { Capabilities } from '@/models/access';
import type { Issue } from '@/models/issue/issue';
import { IssuePermission } from '@/models/issue/issuePermission';
import { IssuePriority } from '@/models/issue/issuePriority';
import { IssueProgress } from '@/models/issue/issueProgress';
import { IssueType } from '@/models/issue/issueType';

type FormValues = Omit<UpdateIssueRequest, 'id'>;

interface Props {
  issue: Issue;
  capabilities: Capabilities;
}

function IssueDetailsSummary({ issue, capabilities }: Props) {
  const { authUser } = useAuth();
  const navigate = useNavigate();
  const [editing, setEditing] = useState(false);
  const [updating, setUpdating] = useState(false);
  const [current, setCurrent] = useState(issue);
  const [pinnedToKanban, setPinnedToKanban] = useState(
    issue.permission?.isPinnedToKanban ?? false
  );

  const {
    register,
    formState: { errors },
    handleSubmit,
    control,
    reset
  } = useForm<FormValues>({
    defaultValues: {
      summary: issue.summary,
      description: issue.description,
      type: issue.type,
      progress: issue.progress,
      priority: issue.priority
    },
    mode: 'all'
  });

  const readOnly = !capabilities.canModify || !editing;

  const handleIssueUpdate = handleSubmit(async (values) => {
    try {
      setUpdating(true);
      const updated = await adapter.Issue.update({ id: issue.id, ...values });
      setCurrent(updated);
      reset({
        summary: updated.summary,
        description: updated.description,
        type: updated.type,
        progress: updated.progress,
        priority: updated.priority
      });
      toast.success('Data updated');
      setEditing(false);
    } catch (error) {
      displayError(error, 'Updating data error');
    } finally {
      setUpdating(false);
    }
  });

  const handlePinnedChange = async (isPinnedToKanban: boolean) => {
    if (authUser?.id === undefined) return;

    const payload = {
      userId: authUser.id,
      issueId: issue.id,
      isPinnedToKanban,
      issuePermission: issue.permission
        ? IssuePermission.CanModify
        : IssuePermission.CanDelete
    };

    try {
      if (issue.permission) {
        await adapter.Permission.update(payload);
      } else {
        await adapter.Permission.create(payload);
      }
      setPinnedToKanban(isPinnedToKanban);
      toast.success(
        isPinnedToKanban
          ? 'Issue added to the kanban board'
          : 'Issue removed from the kanban board'
      );
    } catch (error) {
      displayError(error, 'Updating the kanban board failed');
    }
  };

  const handleIssueRemoval = async () => {
    try {
      await adapter.Issue.delete(issue.id);
      navigate(`/projects/${issue.projectId}/issues`);
      toast.success('Issue removed');
    } catch (error) {
      displayError(error, 'Removing issue error');
    }
  };

  return (
    <>
      <Box>
        <Box display="flex" justifyContent="space-between" alignItems="center">
          <Box
            display="flex"
            justifyContent="space-between"
            alignItems="center"
            sx={{ width: '100%' }}
          >
            <Box
              display="flex"
              justifyContent="space-between"
              alignItems="center"
              sx={{ color: 'text.icon' }}
            >
              <Typography variant="h6">{current.id}</Typography>
              <VerticalDivider />
              <Typography>{current.type}</Typography>
              <VerticalDivider />
              {!updating && (
                <>
                  <IssuePriorityBadge value={current.priority} />
                  <IssueProgressBadge value={current.progress} />
                </>
              )}
            </Box>
            <Box display="flex">
              {capabilities.canDelete && (
                <ButtonIconWithConfirmationDialog
                  hoverOverTitle="Delete Issue"
                  dialogText="Delete Issue?"
                  icon={<DeleteIcon />}
                  onConfirm={handleIssueRemoval}
                />
              )}
              {capabilities.canModify && (
                <>
                  {pinnedToKanban ? (
                    <TooltipActionButton
                      title="Remove from kanban"
                      icon={<BookmarkRemoveIcon />}
                      onClick={() => handlePinnedChange(false)}
                    />
                  ) : (
                    <TooltipActionButton
                      title="Add to kanban"
                      icon={<BookmarkAddIcon />}
                      onClick={() => handlePinnedChange(true)}
                    />
                  )}

                  {editing ? (
                    <Grow in={true}>
                      <Box display="flex">
                        <VerticalDivider />
                        <TooltipActionButton
                          title="Save changes"
                          icon={<SaveIcon />}
                          onClick={handleIssueUpdate}
                        />
                        <TooltipActionButton
                          title="Cancel changes"
                          icon={<CancelIcon />}
                          onClick={() => {
                            setEditing(false);
                          }}
                        />
                      </Box>
                    </Grow>
                  ) : (
                    <TooltipActionButton
                      title="Edit Issue"
                      icon={<EditIcon />}
                      onClick={() => {
                        setEditing(true);
                      }}
                    />
                  )}
                </>
              )}
            </Box>
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
              error={errors.summary !== undefined}
              helperText={errors.summary?.message}
              {...register('summary', SUMMARY_RULE)}
            />
          </FormFieldWrapper>
          <FormFieldWrapper title="Priority" highlighted={editing}>
            <Controller
              control={control}
              name="priority"
              render={({ field }) => (
                <IssuePrioritySelect
                  fullWidth
                  disabled={readOnly}
                  value={field.value}
                  onBlur={field.onBlur}
                  onChange={(value) => {
                    field.onChange(value ?? IssuePriority.Low);
                  }}
                />
              )}
            />
          </FormFieldWrapper>
          <FormFieldWrapper title="Type" highlighted={editing}>
            <Controller
              control={control}
              name="type"
              render={({ field }) => (
                <IssueTypeSelect
                  fullWidth
                  disabled={readOnly}
                  value={field.value}
                  onBlur={field.onBlur}
                  onChange={(value) => {
                    field.onChange(value ?? IssueType.Bug);
                  }}
                />
              )}
            />
          </FormFieldWrapper>
          <FormFieldWrapper title="Progress" highlighted={editing}>
            <Controller
              control={control}
              name="progress"
              render={({ field }) => (
                <IssueProgressSelect
                  fullWidth
                  disabled={readOnly}
                  value={field.value}
                  onBlur={field.onBlur}
                  onChange={(value) => {
                    field.onChange(value ?? IssueProgress.ToDo);
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

export default IssueDetailsSummary;
