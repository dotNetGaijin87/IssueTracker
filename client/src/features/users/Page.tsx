import { useCallback, useState, type ChangeEvent } from 'react';
import { adapter, type UserListCriteria } from '@/adapters/adapter';
import Pagination from '@/components/pagination/Pagination';
import { useAsyncResource } from '@/helpers/useAsyncResource';
import LoadingPage from '@/layout/common/LoadingPage';
import { emptyPage, type Paginated } from '@/models/pagination';
import type { User } from '@/models/user/user';
import UsersSearch from './ActionBar';
import UsersTable from './UsersTable';

const EMPTY: Paginated<User> = emptyPage<User>();

function UsersPage() {
  const [criteria, setCriteria] = useState<UserListCriteria>({});

  const load = useCallback(() => adapter.User.list(criteria), [criteria]);

  const { data, loading } = useAsyncResource(
    load,
    EMPTY,
    'Fetching data error'
  );

  const handlePaginationChange = (
    _event: ChangeEvent<unknown>,
    page: number
  ) => {
    setCriteria((current) => ({ ...current, page }));
  };

  if (loading) return <LoadingPage />;

  return (
    <>
      <UsersSearch onSearch={setCriteria} />
      <UsersTable users={data.items} />
      <Pagination
        pageCount={data.pageCount}
        page={data.page}
        onChange={handlePaginationChange}
      />
    </>
  );
}

export default UsersPage;
