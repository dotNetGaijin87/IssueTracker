import { useState } from 'react';
import { Box, Divider, Grow, TextField, Typography } from '@mui/material';
import { useParams } from 'react-router';
import { useNavigate } from 'react-router-dom';
import { adapter } from '../../../adapters/adapter';
import DeleteIcon from '@mui/icons-material/Delete';
import SaveIcon from '@mui/icons-material/Save';
import EditIcon from '@mui/icons-material/Edit';
import BookmarkAddIcon from '@mui/icons-material/BookmarkAdd';
import BookmarkRemoveIcon from '@mui/icons-material/BookmarkRemove';
import CancelIcon from '@mui/icons-material/Cancel';
import { toast } from 'react-toastify';
import displayError from '../../../helpers/errorHandling/displayError';
import { Controller, useForm } from 'react-hook-form';
import FormFieldWrapper from '../../../components/formFieldWrapper/FormFieldWrapper';
import safelyConvertDateTime from '../../../helpers/time/safelyConvertDateTime';
import MarkupEditor from '../../../components/markupEditor/MarkupEditor';
import Panel from '../../../components/panel/Panel';
import TooltipActionButton from '../../../components/tooltipActionButton/TooltipActionButton';
import ButtonIconWithConfirmationDialog from '../../../components/buttonIconWithConfirmationDialog/ButtonIconWithConfirmationDialog';
import { Issue } from '../../../models/issue/issue';
import { IssueProgress } from '../../../models/issue/issueProgress';
import IssueProgressBadge from '../../../components/issueProgress/IssueProgressBadge';
import IssueProgressSelect from '../../../components/issueProgress/IssueProgressSelect';
import { IssueType } from '../../../models/issue/issueType';
import IssueTypeSelect from '../../../components/issueType/IssueTypeSelect';
import { IssuePriority } from '../../../models/issue/issuePriority';
import IssuePrioritySelect from '../../../components/issuePriority/IssuePrioritySelect';
import IssuePriorityBadge from '../../../components/issuePriority/IssuePriorityBadge';
import VerticalDivider from '../../../components/verticalDivider/VerticalDivider';
import { IssuePermission } from '../../../models/issue/issuePermission';
import { useAuth } from '../../../authentication/Auth';
import { Permission } from '../../../models/permission/permission';
import hasAdminOrManagerRole from '../../../helpers/auth/hasAdminOrManagerRole';
import parseDateTimeToMessage from '../../../helpers/time/parseDateTimeToMessage';

interface Props {
  issue: Issue | undefined;
  initialPermission?: Permission;
}

function IssueDetailsSummary({ issue, initialPermission }: Props): JSX.Element {
  const { authUser } = useAuth();
  const navigate = useNavigate();
  const [editing, setEditing] = useState(false);
  const [updating, setUpdating] = useState(false);
  const { projectId } = useParams<{ projectId: string }>();
  const [pinnedToKanban, setPinnedToKanban] = useState(
    initialPermission?.isPinnedToKanban
  );

  const isAdminOrManager = hasAdminOrManagerRole(authUser?.role);

  const evaluatePermission = () => {
    let permissions: IssuePermission[] = [];
    if (
      initialPermission?.issuePermission === IssuePermission.CanDelete ||
      isAdminOrManager
    ) {
      permissions.push(IssuePermission.CanDelete);
      permissions.push(IssuePermission.CanModify);
      return permissions;
    } else if (
      initialPermission?.issuePermission === IssuePermission.CanModify ||
      isAdminOrManager
    ) {
      permissions.push(IssuePermission.CanModify);
    }

    return permissions;
  };
  const permissions = evaluatePermission();

  const {
    register,
    formState: { errors },
    handleSubmit,
    control,
    getValues,
    reset
  } = useForm<Issue>({
    defaultValues: issue,
    mode: 'all'
  });

  const handleIssueUpdate = async () => {
    setUpdating(true);
    await handleSubmit(async (data) => {
      try {
        let issue = await adapter.Issue.update({
          id: data.id,
          summary: data.summary,
          description: data.description,
          type: data.type,
          progress: data.progress,
          priority: data.priority
        });
        reset(issue);
        toast.success('Data updated');
      } catch (ex) {
        displayError(ex, 'Updating data error');
      }
      setUpdating(false);
      setEditing(false);
    })();
  };

  const handlePermissionUpdate = async (isPinnedToKanban: boolean) => {
    await handleSubmit(async (data) => {
      try {
        if (!initialPermission && isAdminOrManager) {
          await adapter.Permission.create({
            userId: authUser?.name,
            issueId: issue?.id,
            isPinnedToKanban: isPinnedToKanban,
            issuePermission: IssuePermission.CanDelete
          });
        } else {
          await adapter.Permission.update({
            userId: authUser?.name,
            issueId: issue?.id,
            isPinnedToKanban: isPinnedToKanban,
            issuePermission: IssuePermission.CanModify
          });
        }

        setPinnedToKanban(isPinnedToKanban);
        isPinnedToKanban
          ? toast.success('Issue added to the kanban board')
          : toast.success('Issue removed from the kanban board');
      } catch (ex) {
        displayError(ex, 'Removing issue from the kanban board error');
      }
    })();
  };

  const handleIssueRemoval = async (name: string) => {
    try {
      await adapter.Issue.delete(name);
      navigate(`/projects/${projectId}/issues`);
      toast.success('Issue removed');
    } catch (ex: any) {
      displayError(ex, 'Removing issue error');
    }
  };

  const handleEditing = () => {
    setEditing(true);
  };

  const handleCancelEditing = () => {
    setEditing(false);
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
              <Typography variant="h6">{getValues('id')}</Typography>
              <VerticalDivider />
              <Typography>{getValues('type')}</Typography>
              <VerticalDivider />
              {!updating && (
                <>
                  <IssuePriorityBadge
                    value={getValues('priority') as IssuePriority}
                  />

                  <IssueProgressBadge
                    value={getValues('progress') as IssueProgress}
                  />
                </>
              )}
            </Box>
            <Box display="flex">
              {permissions.includes(IssuePermission.CanDelete) && (
                <>
                  <ButtonIconWithConfirmationDialog
                    hoverOverTitle={'Delete Issue'}
                    dialogText={'Delete Issue?'}
                    icon={<DeleteIcon />}
                    onConfirm={async () => {
                      await handleIssueRemoval(getValues('id'));
                    }}
                  />
                </>
              )}
              {permissions.includes(IssuePermission.CanModify) && (
                <>
                  {pinnedToKanban ? (
                    <TooltipActionButton
                      title={'Remove from kanban'}
                      icon={<BookmarkRemoveIcon />}
                      onClick={async () => await handlePermissionUpdate(false)}
                    />
                  ) : (
                    <TooltipActionButton
                      title={'Add to kanban'}
                      icon={<BookmarkAddIcon />}
                      onClick={async () => await handlePermissionUpdate(true)}
                    />
                  )}

                  {editing ? (
                    <Grow in={true}>
                      <Box display="flex">
                        <VerticalDivider />
                        <TooltipActionButton
                          title={'Save changes'}
                          icon={<SaveIcon />}
                          onClick={handleIssueUpdate}
                        />
                        <TooltipActionButton
                          title={'Cancel changes'}
                          icon={<CancelIcon />}
                          onClick={handleCancelEditing}
                        />
                      </Box>
                    </Grow>
                  ) : (
                    <TooltipActionButton
                      title={'Edit Issue'}
                      icon={<EditIcon />}
                      onClick={handleEditing}
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
            {`Created ${parseDateTimeToMessage(
              getValues('creationTime')
            )} by ${getValues('createdBy')}`}
          </Typography>
        </Box>
      </Box>
      <Panel>
        <div style={{ width: '40%' }}>
          <FormFieldWrapper title="Summary" highlighted={editing}>
            <TextField
              fullWidth
              disabled={
                !permissions.includes(IssuePermission.CanModify) || !editing
              }
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
          <FormFieldWrapper title="Priority" highlighted={editing}>
            <Controller
              control={control}
              name="priority"
              render={({ field }) => (
                <IssuePrioritySelect
                  args={field}
                  fullWidth
                  disabled={
                    !permissions.includes(IssuePermission.CanModify) || !editing
                  }
                  defaultValue={IssuePriority.Low}
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
                  args={field}
                  fullWidth
                  disabled={
                    !permissions.includes(IssuePermission.CanModify) || !editing
                  }
                  defaultValue={IssueType.Bug}
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
                  args={field}
                  fullWidth
                  disabled={
                    !permissions.includes(IssuePermission.CanModify) || !editing
                  }
                  defaultValue={IssueProgress.Canceled}
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
        </div>
        <Controller
          control={control}
          name="description"
          render={({ field: { onChange, onBlur, value } }) => (
            <MarkupEditor
              title="Description"
              value={value}
              disabled={
                !permissions.includes(IssuePermission.CanModify) || !editing
              }
              onBlur={onBlur}
              onChange={onChange}
            />
          )}
        />
      </Panel>
    </>
  );
}

export default IssueDetailsSummary;
