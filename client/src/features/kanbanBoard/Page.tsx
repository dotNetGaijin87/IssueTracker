import { useEffect, useState } from 'react';
import { adapter } from '../../adapters/adapter';
import { useAuth } from '../../authentication/Auth';
import displayError from '../../helpers/errorHandling/displayError';
import LoadingPage from '../../layout/common/LoadingPage';
import Kanban from '../../components/kanban/Kanban';
import { KanbanCard } from '../../models/kanbanCard/kanbanCard';

function KanbanBoard(): JSX.Element {
  const { authUser } = useAuth();
  const [kanbanCards, setKanbanCards] = useState<KanbanCard[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const run = async () => {
      try {
        setLoading(true);
        let cards = await adapter.Issue.getIssueKanban();
        setKanbanCards(cards);
      } catch (ex) {
        displayError(ex, 'Getting kanban data error');
      }
      setLoading(false);
    };

    run();
  }, [authUser?.name]);

  if (loading) return <LoadingPage />;

  return <Kanban cards={kanbanCards} />;
}

export default KanbanBoard;
