import { Box, Grid, Typography } from '@mui/material';
import Board, {
  type DragDestination,
  type DragSource,
  type KanbanBoard
} from '@asseinfo/react-kanban';
import FeaturedPlayListIcon from '@mui/icons-material/FeaturedPlayList';
import {
  adapter,
  type KanbanPositionUpdate,
  type UpdateKanbanRequest
} from '@/adapters/adapter';
import IssuePriorityBadge from '@/components/issuePriority/IssuePriorityBadge';
import IssueTypeIcon from '@/components/issueType/IssueTypeIcon';
import KanbanCardShell from '@/components/kanbanCard/KanbanCard';
import TooltipNavButtonBase from '@/components/toolTipNavButton/TooltipNavButtonBase';
import displayError from '@/helpers/errorHandling/displayError';
import type { IssueId, ProjectId } from '@/models/ids';
import {
  issueProgresses,
  type IssueProgress
} from '@/models/issue/issueProgress';
import type { IssuePriority } from '@/models/issue/issuePriority';
import type { IssueType } from '@/models/issue/issueType';
import type { KanbanCard } from '@/models/kanbanCard/kanbanCard';

const MAX_TITLE_LENGTH = 15;
const MAX_CONTENT_LENGTH = 70;

type BoardCard = {
  id: IssueId;
  projectId: ProjectId;
  position: number;
  title: string;
  content: string;
  priority: IssuePriority;
  type: IssueType;
};

interface Props {
  cards: KanbanCard[];
}

function truncate(value: string, maxLength: number): string {
  return value.length > maxLength ? `${value.slice(0, maxLength)}...` : value;
}

function toBoard(cards: KanbanCard[]): KanbanBoard<BoardCard> {
  return {
    columns: issueProgresses.map((progress) => ({
      id: progress,
      title: progress,
      cards: cards
        .filter((card) => card.progress === progress)
        .sort((a, b) => a.position - b.position)
        .map((card) => ({
          id: card.id,
          projectId: card.projectId,
          position: card.position,
          title: card.id,
          content: card.summary,
          priority: card.priority,
          type: card.type
        }))
    }))
  };
}

/** Both affected columns are renumbered so positions survive a reload. */
function toPositionUpdates(
  board: KanbanBoard<BoardCard>,
  source: DragSource,
  destination: DragDestination
): KanbanPositionUpdate[] {
  return board.columns
    .filter(
      (column) =>
        column.id === source.fromColumnId ||
        column.id === destination.toColumnId
    )
    .flatMap((column) =>
      column.cards.map((card, index) => ({
        issueId: card.id,
        kanbanRowPosition: index,
        isPinnedToKanban: true
      }))
    );
}

function Kanban({ cards }: Props) {
  const handleCardDragEnd = (
    board: KanbanBoard<BoardCard>,
    card: BoardCard,
    source: DragSource,
    destination: DragDestination
  ) => {
    const progress = issueProgresses.find(
      (value): value is IssueProgress => value === destination.toColumnId
    );
    if (progress === undefined) return;

    const request: UpdateKanbanRequest = {
      issueId: card.id,
      progress,
      permissions: toPositionUpdates(board, source, destination)
    };

    adapter.Issue.updateKanban(request).catch((error: unknown) => {
      displayError(error, 'Updating data error');
    });
  };

  return (
    <Grid container display="flex" justifyContent="center">
      <Board<BoardCard>
        onCardDragEnd={handleCardDragEnd}
        initialBoard={toBoard(cards)}
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
        renderCard={({ content, id, projectId, priority, type, title }) => (
          <KanbanCardShell>
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
                    sx={{ bgcolor: 'background.default', borderRadius: 1 }}
                  >
                    <IssueTypeIcon value={type} />
                    <IssuePriorityBadge value={priority} variant="text" />
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
                    {truncate(title, MAX_TITLE_LENGTH)}
                  </Typography>
                  <Typography
                    sx={{ m: 1, color: 'text.icon', overflowWrap: 'anywhere' }}
                  >
                    {truncate(content, MAX_CONTENT_LENGTH)}
                  </Typography>
                </Box>
              </Box>
            </Box>
          </KanbanCardShell>
        )}
      />
    </Grid>
  );
}

export default Kanban;
