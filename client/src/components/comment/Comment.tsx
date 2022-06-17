import { useState } from 'react';
import {
  Avatar,
  Box,
  Stack,
  TextareaAutosize,
  Typography
} from '@mui/material';
import { IssueComment } from '../../models/comment/issueComment';
import parseDateTimeToMessage from '../../helpers/time/parseDateTimeToMessage';
import { adapter } from '../../adapters/adapter';
import { useAuth } from '../../authentication/Auth';
import CommentButton from '../commentButton/CommentButton';
import { UserRole } from '../../models/user/userRole';

interface Props {
  comment: IssueComment;
  onCommentStateChanged?: () => void;
}

function Comment({ comment, onCommentStateChanged }: Props) {
  const { authUser } = useAuth();
  const [content, setContent] = useState(comment?.content);
  const [editing, setEditing] = useState(false);
  const [saving, setSaving] = useState(false);
  const [deleting, setDeleting] = useState(false);
  const canModify =
    authUser?.name === comment.userId || authUser?.role === UserRole.admin;

  const handleSaveComment = async () => {
    try {
      setSaving(true);
      await adapter.Comment.update({
        id: comment.id,
        content: content
      });
      if (onCommentStateChanged) onCommentStateChanged();
    } catch (ex) {}
    setSaving(false);
  };

  const handleDeleteComment = async () => {
    try {
      setDeleting(true);
      await adapter.Comment.delete(comment.id);
      if (onCommentStateChanged) onCommentStateChanged();
    } catch (ex) {}
    setDeleting(false);
  };

  const handleEditComment = () => {
    setEditing(true);
  };

  const handleCancelEditingComment = () => {
    setEditing(false);
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
            {comment?.userId.substring(0, 2).toUpperCase()}
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
            onChange={(e) => setContent(e.target.value)}
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
                onClick={handleSaveComment}
              />
              <CommentButton
                label="Cancel"
                color="secondary"
                loading={false}
                onClick={handleCancelEditingComment}
              />
            </>
          ) : (
            <>
              <CommentButton
                label="Edit"
                loading={false}
                onClick={handleEditComment}
              />
              <CommentButton
                label="Delete"
                loading={deleting}
                onClick={handleDeleteComment}
              />
            </>
          )}
        </Box>
      )}
    </Box>
  );
}

export default Comment;
