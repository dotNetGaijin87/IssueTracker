/**
 * `@asseinfo/react-kanban` ships no type declarations and is absent from
 * DefinitelyTyped, so this describes the surface this app actually uses.
 */
declare module '@asseinfo/react-kanban' {
  import type { ReactNode } from 'react';

  export interface KanbanColumn<TCard> {
    id: string;
    title: string;
    cards: TCard[];
  }

  export interface KanbanBoard<TCard> {
    columns: KanbanColumn<TCard>[];
  }

  export interface DragSource {
    fromPosition: number;
    fromColumnId: string;
  }

  export interface DragDestination {
    toPosition: number;
    toColumnId: string;
  }

  export interface RenderCardOptions {
    dragging: boolean;
  }

  export interface BoardProps<TCard> {
    initialBoard?: KanbanBoard<TCard>;
    children?: KanbanBoard<TCard>;
    disableCardDrag?: boolean;
    disableColumnDrag?: boolean;
    renderCard?: (card: TCard, options: RenderCardOptions) => ReactNode;
    renderColumnHeader?: (column: KanbanColumn<TCard>) => ReactNode;
    onCardDragEnd?: (
      board: KanbanBoard<TCard>,
      card: TCard,
      source: DragSource,
      destination: DragDestination
    ) => void;
  }

  export default function Board<TCard>(props: BoardProps<TCard>): JSX.Element;
}
