import { useState } from 'react';
import {
  Avatar,
  Box,
  Stack,
  TextareaAutosize,
  Typography
} from '@mui/material';
import { adapter } from '@/adapters/adapter';
import { useAuth } from '@/authentication/Auth';
import CommentButton from '@/components/commentButton/CommentButton';
import displayError from '@/helpers/errorHandling/displayError';
import parseDateTimeToMessage from '@/helpers/time/parseDateTimeToMessage';
import type { IssueComment } from '@/models/comment/issueComment';
import { UserRole } from '@/models/user/userRole';

const AVATAR_INITIALS = 2;

interface Props {
  comment: IssueComment;
  onCommentStateChanged?: () => void;
}

function Comment({ comment, onCommentStateChanged }: Props) {
  const { authUser } = useAuth();
  const [content, setContent] = useState(comment.content);
  const [editing, setEditing] = useState(false);
  const [saving, setSaving] = useState(false);
  const [deleting, setDeleting] = useState(false);

  const canModify =
    authUser?.id === comment.userId || authUser?.role === UserRole.admin;

  const handleSaveComment = async () => {
    try {
      setSaving(true);
      await adapter.Comment.update({ id: comment.id, content });
      setEditing(false);
      onCommentStateChanged?.();
    } catch (error) {
      displayError(error, 'Updating comment failed');
    } finally {
      setSaving(false);
    }
  };

  const handleDeleteComment = async () => {
    try {
      setDeleting(true);
      await adapter.Comment.delete(comment.id);
      onCommentStateChanged?.();
    } catch (error) {
      displayError(error, 'Deleting comment failed');
    } finally {
      setDeleting(false);
    }
  };

  return (
    <Box sx={{ width: 'fit-content', mb: 3 }}>
      <Box display="flex" justifyContent="space-between" alignItems="center">
        <Typography>{comment.userId}</Typography>
        <Typography component="span" sx={{ color: 'text.icon' }}>
          {parseDateTimeToMessage(comment.creationTime)}
        </Typography>
      </Box>
      <Box display="flex">
        <Stack>
          <Avatar alt="User">
            {comment.userId.substring(0, AVATAR_INITIALS).toUpperCase()}
          </Avatar>
        </Stack>
        <Box
          sx={{
            backgroundColor: 'background.paper',
            width: 'fit-content',
            ml: 0.5,
            borderRadius: 1
          }}
        >
          <TextareaAutosize
            disabled={!canModify || !editing}
            minRows={4}
            value={content}
            onChange={(event) => {
              setContent(event.target.value);
            }}
            style={{
              backgroundColor: 'inherit',
              color: 'inherit',
              minWidth: 500,
              maxWidth: 800,
              maxHeight: 400
            }}
          />
        </Box>
      </Box>

      {canModify && (
        <Box display="flex" justifyContent="end">
          {editing ? (
            <>
              <CommentButton
                label="Save"
                color="secondary"
                loading={saving}
                onClick={() => {
                  void handleSaveComment();
                }}
              />
              <CommentButton
                label="Cancel"
                color="secondary"
                loading={false}
                onClick={() => {
                  setContent(comment.content);
                  setEditing(false);
                }}
              />
            </>
          ) : (
            <>
              <CommentButton
                label="Edit"
                loading={false}
                onClick={() => {
                  setEditing(true);
                }}
              />
              <CommentButton
                label="Delete"
                loading={deleting}
                onClick={() => {
                  void handleDeleteComment();
                }}
              />
            </>
          )}
        </Box>
      )}
    </Box>
  );
}

export default Comment;
