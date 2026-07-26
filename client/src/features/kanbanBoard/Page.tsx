import { useCallback } from 'react';
import { adapter } from '@/adapters/adapter';
import { useAuth } from '@/authentication/Auth';
import Kanban from '@/components/kanban/Kanban';
import { useAsyncResource } from '@/helpers/useAsyncResource';
import LoadingPage from '@/layout/common/LoadingPage';
import type { KanbanCard } from '@/models/kanbanCard/kanbanCard';

const NO_CARDS: KanbanCard[] = [];

function KanbanBoard() {
  const { authUser } = useAuth();

  const load = useCallback(
    () => adapter.Issue.getKanban(),
    // The board is scoped to the signed-in user server-side.
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [authUser?.id]
  );

  const { data: cards, loading } = useAsyncResource(
    load,
    NO_CARDS,
    'Getting kanban data error'
  );

  if (loading) return <LoadingPage />;

  return <Kanban cards={cards} />;
}

export default KanbanBoard;
