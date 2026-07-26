import { useEffect, useRef, useState } from 'react';
import { Box, Grow, TextField } from '@mui/material';
import type { UserListCriteria } from '@/adapters/adapter';
import Bar from '@/components/bar/Bar';
import Field from '@/components/field/Field';
import delayExec from '@/helpers/delayExec';

const SEARCH_DEBOUNCE_MS = 1500;

interface Props {
  onSearch: (criteria: UserListCriteria) => void;
}

function ActionBar({ onSearch }: Props) {
  const [id, setId] = useState('');
  const [email, setEmail] = useState('');

  const isInitialRender = useRef(true);
  const onSearchRef = useRef(onSearch);
  onSearchRef.current = onSearch;

  useEffect(() => {
    if (isInitialRender.current) {
      isInitialRender.current = false;
      return;
    }

    return delayExec(() => {
      onSearchRef.current({ id, email });
    }, SEARCH_DEBOUNCE_MS);
  }, [id, email]);

  return (
    <Grow in={true}>
      <Box display="flex" alignItems="center" justifyContent="space-between">
        <Bar title="Filter">
          <Field>
            <TextField
              size="small"
              value={id}
              onChange={(event) => {
                setId(event.target.value);
              }}
              label="Name"
            />
          </Field>
          <Field>
            <TextField
              size="small"
              value={email}
              onChange={(event) => {
                setEmail(event.target.value);
              }}
              label="Email"
            />
          </Field>
        </Bar>
      </Box>
    </Grow>
  );
}

export default ActionBar;
