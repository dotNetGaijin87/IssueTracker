import { useCallback, useState, type ChangeEvent } from 'react';
import { Box } from '@mui/material';
import { adapter } from '@/adapters/adapter';
import AddComment from '@/components/addComment/AddComment';
import Comment from '@/components/comment/Comment';
import Pagination from '@/components/pagination/Pagination';
import { useIssueId } from '@/helpers/routing/useRouteId';
import { useAsyncResource } from '@/helpers/useAsyncResource';
import LoadingPage from '@/layout/common/LoadingPage';
import type { Capabilities } from '@/models/access';
import type { IssueComment } from '@/models/comment/issueComment';
import { emptyPage, type Paginated } from '@/models/pagination';

const EMPTY: Paginated<IssueComment> = emptyPage<IssueComment>();

interface Props {
  capabilities: Capabilities;
}

function IssueDetailsComments({ capabilities }: Props) {
  const issueId = useIssueId();
  const [page, setPage] = useState(1);

  const load = useCallback(
    () => adapter.Comment.list({ issueId, page }),
    [issueId, page]
  );

  const { data, loading, reload } = useAsyncResource(
    load,
    EMPTY,
    'Getting data error'
  );

  const handlePaginationChange = (
    _event: ChangeEvent<unknown>,
    nextPage: number
  ) => {
    setPage(nextPage);
  };

  if (loading) return <LoadingPage />;

  return (
    <>
      <Box>
        {capabilities.canModify && <AddComment onCommentAdded={reload} />}

        {data.items.map((comment) => (
          <Comment
            key={comment.id}
            comment={comment}
            onCommentStateChanged={reload}
          />
        ))}
      </Box>
      <Pagination
        pageCount={data.pageCount}
        page={data.page}
        onChange={handlePaginationChange}
      />
    </>
  );
}

export default IssueDetailsComments;
