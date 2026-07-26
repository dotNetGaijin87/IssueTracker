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
import { useIssueId } from '@/helpers/routing/useRouteId';

const AVATAR_INITIALS = 2;

interface Props {
  onCommentAdded?: () => void;
}

function AddComment({ onCommentAdded }: Props) {
  const { authUser } = useAuth();
  const issueId = useIssueId();
  const [content, setContent] = useState('');
  const [saving, setSaving] = useState(false);

  const handleAddComment = async () => {
    if (issueId === undefined || authUser?.id === undefined) return;

    try {
      setSaving(true);
      await adapter.Comment.create({ userId: authUser.id, issueId, content });
      setContent('');
      onCommentAdded?.();
    } catch (error) {
      displayError(error, 'Adding comment failed');
    } finally {
      setSaving(false);
    }
  };

  return (
    <Box sx={{ width: 'fit-content', mb: 3 }}>
      <Box display="flex" justifyContent="space-between" alignItems="center">
        <Typography>{authUser?.name}</Typography>
      </Box>
      <Box display="flex">
        <Stack>
          <Avatar alt="User">
            {authUser?.name?.substring(0, AVATAR_INITIALS).toUpperCase()}
          </Avatar>
        </Stack>
        <Box
          sx={{
            backgroundColor: 'primary.dark',
            width: 'fit-content',
            ml: 0.5,
            borderRadius: 1
          }}
        >
          <TextareaAutosize
            minRows={4}
            value={content}
            onChange={(event) => {
              setContent(event.target.value);
            }}
            style={{
              backgroundColor: 'inherit',
              minWidth: 500,
              maxWidth: 800,
              maxHeight: 400
            }}
          />
        </Box>
      </Box>
      <Box display="flex" justifyContent="end">
        <CommentButton
          label="Add"
          loading={saving}
          onClick={() => {
            void handleAddComment();
          }}
        />
        <CommentButton
          label="Clear"
          loading={false}
          onClick={() => {
            setContent('');
          }}
        />
      </Box>
    </Box>
  );
}

export default AddComment;
