import React, { useEffect, useState } from 'react';
import { Box } from '@mui/material';
import { useParams } from 'react-router-dom';
import Pagination from '../../../components/pagination/Pagination';
import Comment from '../../../components/comment/Comment';
import { IssuePermission } from '../../../models/issue/issuePermission';
import { IssueComment } from '../../../models/comment/issueComment';
import { adapter } from '../../../adapters/adapter';
import LoadingPage from '../../../layout/common/LoadingPage';
import AddComment from '../../../components/addComment/AddComment';
import displayError from '../../../helpers/errorHandling/displayError';

interface Props {
  permissions: IssuePermission[];
}

function IssueDetailsComments({ permissions }: Props) {
  const { issueId } = useParams<{ issueId: string }>();
  const [loading, setLoading] = React.useState(false);
  const [comments, setComments] = React.useState<IssueComment[]>([]);
  const [page, setPage] = React.useState(1);
  const [pageCount, setPageCount] = React.useState(1);
  const [searchCriteria, setSearchCriteria] = useState<any>({
    IssueId: issueId
  });
  const canModifyPermission = permissions.includes(IssuePermission.CanModify);

  useEffect(() => {
    const run = async () => {
      try {
        setLoading(true);
        let commentVm = await adapter.Comment.list(searchCriteria);
        setComments(commentVm.comments);
        setPage(commentVm.page);
        setPageCount(commentVm.pageCount);
      } catch (ex) {
        displayError(ex, 'Getting data error');
      }
      setLoading(false);
    };

    run();
  }, [searchCriteria]);

  const handlePaginationChange = (
    event: React.ChangeEvent<unknown>,
    page: number
  ) => {
    setSearchCriteria({
      ...searchCriteria,
      Page: page
    });
  };

  const handleCommentAdded = () => {
    setSearchCriteria({
      ...searchCriteria
    });
  };

  const handleCommentStateChanged = () => {
    setSearchCriteria({
      ...searchCriteria
    });
  };

  if (loading) return <LoadingPage />;

  return (
    <>
      <Box>
        {canModifyPermission && (
          <AddComment onCommentAdded={handleCommentAdded} />
        )}

        {comments?.map((comment) => (
          <Comment
            comment={comment}
            onCommentStateChanged={handleCommentStateChanged}
          />
        ))}
      </Box>
      <Pagination
        pageCount={pageCount}
        page={page}
        onChange={handlePaginationChange}
      />
    </>
  );
}

export default IssueDetailsComments;
