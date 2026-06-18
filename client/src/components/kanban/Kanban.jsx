import { Box, Grid } from '@mui/material';
import Board from '@asseinfo/react-kanban';
import KanbanColumn from '../kanbanCard/KanbanCard';
import issueProgressList from '../issueProgress/issueProgress/issueProgressList';
import IssuePriorityBadge from '../issuePriority/IssuePriorityBadge';
import FeaturedPlayListIcon from '@mui/icons-material/FeaturedPlayList';
import IssueTypeIcon from '../issueType/IssueTypeIcon';
import TooltipNavButtonBase from '../toolTipNavButton/TooltipNavButtonBase';
import { Typography } from '@mui/material';
import { IssueProgress } from '../../models/issue/issueProgress';
import { adapter } from '../../adapters/adapter';
import displayError from '../../helpers/errorHandling/displayError';

const Kanban = ({ cards }) => {
  let board = { columns: [] };

  const updateKanban = async (id, progress, permissions) => {
    try {
      await adapter.Issue.updateIssueKanban({
        issueId: id,
        progress: progress,
        permissions: permissions
      });
    } catch (ex) {
      displayError(ex, 'Updating data error');
    }
  };

  const handleCardDragEnd = (board, card, source, destination) => {
    let permissions = [].concat(
      ...board.columns
        .filter(
          (x) => x.id === source.fromColumnId || x.id === destination.toColumnId
        )
        .map((x) =>
          x.cards.map((x, i) => ({
            issueId: x.id,
            kanbanRowPosition: i,
            isPinnedToKanban: true
          }))
        )
    );

    updateKanban(card.id, destination.toColumnId, permissions);
  };

  if (cards.length > 0) {
    board = {
      columns: [
        ...issueProgressList()
          .filter((p) => p.value !== IssueProgress.Unspecified)
          .map((progressStatus) => {
            const sortedCards = cards
              .filter((x) => x.progress === progressStatus.value)
              .sort((x) => x.position)
              .map((p) => ({
                id: p.id,
                projectId: p.projectId,
                position: p.position,
                title: p.id,
                content: p.summary,
                priority: p.priority,
                type: p.type
              }));

            return {
              id: progressStatus.value,
              title: progressStatus.value,
              cards: [...sortedCards]
            };
          })
      ]
    };
  }

  return (
    <Grid container display="flex" justifyContent="center">
      <Board
        onCardDragEnd={handleCardDragEnd}
        renderColumnHeader={({ title }) => (
          <Box
            sx={{
              m: 1,
              borderWidth: 0,
              borderStyle: 'solid',
              borderBottomWidth: 1,
              borderBottomColor: 'text.icon'
            }}
          >
            <Typography sx={{ m: 0, color: 'text.icon' }}>{title}</Typography>
          </Box>
        )}
        renderCard={(
          { content, id, projectId, priority, type, title },
          { dragging }
        ) => (
          <div dragging={dragging}>
            <KanbanColumn>
              <Box
                display="flex"
                flexDirection="column"
                justifyContent="space-between"
                alignContent="stretch"
              >
                <Box>
                  <Box sx={{ p: 0.2, m: 0 }}>
                    <Box
                      display="flex"
                      justifyContent="space-around"
                      alignItems="center"
                      sx={{
                        bgcolor: 'background.default',
                        borderRadius: 1
                      }}
                    >
                      <IssueTypeIcon value={type} />
                      <IssuePriorityBadge value={priority} borderless={true} />
                      <TooltipNavButtonBase
                        title="Show details"
                        routeTo={`/projects/${projectId}/issues/${id}`}
                        icon={
                          <FeaturedPlayListIcon sx={{ color: 'text.icon' }} />
                        }
                      />
                    </Box>
                  </Box>
                  <Box>
                    <Typography sx={{ m: 1, overflowWrap: 'anywhere' }}>
                      {title.length > 15
                        ? title.substring(0, 15) + '...'
                        : title}
                    </Typography>
                    <Typography
                      sx={{
                        m: 1,
                        color: 'text.icon',
                        overflowWrap: 'anywhere'
                      }}
                    >
                      {content?.length > 70
                        ? content.substring(0, 70) + '...'
                        : content}
                    </Typography>
                  </Box>
                </Box>
              </Box>
            </KanbanColumn>
          </div>
        )}
        initialBoard={board}
      />
    </Grid>
  );
};

export default Kanban;
