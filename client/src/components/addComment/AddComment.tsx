import { useState } from 'react';
import {
  Avatar,
  Box,
  Stack,
  TextareaAutosize,
  Typography
} from '@mui/material';
import { useAuth } from '../../authentication/Auth';
import { adapter } from '../../adapters/adapter';
import { useParams } from 'react-router-dom';
import CommentButton from '../commentButton/CommentButton';

interface Props {
  onCommentAdded?: () => void;
}

function AddComment({ onCommentAdded }: Props) {
  const { authUser } = useAuth();
  const { issueId } = useParams<{ issueId: string }>();
  const [content, setContent] = useState('');
  const [loading, setLoading] = useState(false);

  const handleAddComment = async () => {
    try {
      setLoading(true);
      await adapter.Comment.create({
        userId: authUser?.name,
        issueId: issueId,
        content: content
      });
      if (onCommentAdded) onCommentAdded();
    } catch (ex) {}
    setLoading(false);
  };

  const handleClearComment = async () => {
    setContent('');
  };

  return (
    <Box sx={{ width: 'fit-content', mb: 3 }}>
      <Box display="flex" justifyContent="space-between" alignItems="center">
        <Typography>{authUser?.name}</Typography>
      </Box>
      <Box display="flex">
        <Stack>
          <Avatar alt="User">
            {authUser?.name?.substring(0, 2).toUpperCase()}
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
            onChange={(e) => setContent(e.target.value)}
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
          loading={loading}
          onClick={handleAddComment}
        />
        <CommentButton
          label="Clear"
          loading={false}
          onClick={handleClearComment}
        />
      </Box>
    </Box>
  );
}

export default AddComment;
