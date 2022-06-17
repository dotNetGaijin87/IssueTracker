import { useEffect, useState } from 'react';
import { adapter } from '../../adapters/adapter';
import { User } from '../../models/user/user';
import UsersTable from './UsersTable';
import UsersSearch from './ActionBar';
import React from 'react';
import LoadingPage from '../../layout/common/LoadingPage';
import Pagination from '../../components/pagination/Pagination';
import displayError from '../../helpers/errorHandling/displayError';

function UsersPage() {
  const [lodaing, setLoading] = React.useState(false);
  const [users, setUsers] = React.useState<User[]>([]);
  const [pageCount, setPageCount] = React.useState(1);
  const [page, setPage] = React.useState(1);
  const [searchCriteria, setSearchCriteria] = useState<any>();

  useEffect(() => {
    const run = async () => {
      try {
        setLoading(true);
        let resp = await adapter.User.list(searchCriteria);
        setUsers(resp.users);
        setPageCount(resp.pageCount);
        setPage(resp.page);
      } catch (ex) {
        displayError(ex, 'Fetching data error');
      }
      setLoading(false);
    };

    run();
  }, [searchCriteria]);

  const handleSearchButtonClicked = (value: object) => {
    setSearchCriteria(value);
  };
  const handlePaginationChange = (
    event: React.ChangeEvent<unknown>,
    page: number
  ) => {
    setSearchCriteria({ ...searchCriteria, page: page });
  };

  if (lodaing) return <LoadingPage />;

  return (
    <>
      <UsersSearch onSearchClicked={handleSearchButtonClicked} />
      <UsersTable users={users} />
      <Pagination
        pageCount={pageCount}
        page={page}
        onChange={handlePaginationChange}
      />
    </>
  );
}

export default UsersPage;
