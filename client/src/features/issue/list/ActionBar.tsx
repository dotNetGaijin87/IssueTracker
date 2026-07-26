import { useEffect, useRef, useState } from 'react';
import { Box, Grow, TextField } from '@mui/material';
import AutorenewOutlinedIcon from '@mui/icons-material/AutorenewOutlined';
import type { IssueListCriteria } from '@/adapters/adapter';
import Bar from '@/components/bar/Bar';
import Field from '@/components/field/Field';
import IssuePrioritySelect from '@/components/issuePriority/IssuePrioritySelect';
import IssueProgressSelect from '@/components/issueProgress/IssueProgressSelect';
import IssueTypeSelect from '@/components/issueType/IssueTypeSelect';
import TooltipActionButton from '@/components/tooltipActionButton/TooltipActionButton';
import delayExec from '@/helpers/delayExec';
import type { IssuePriority } from '@/models/issue/issuePriority';
import type { IssueProgress } from '@/models/issue/issueProgress';
import type { IssueType } from '@/models/issue/issueType';
import AddNewIssue from './AddNewIssue';

const SEARCH_DEBOUNCE_MS = 1500;

interface Props {
  onSearch: (criteria: IssueListCriteria) => void;
}

function ActionBar({ onSearch }: Props) {
  const [id, setId] = useState('');
  const [createdBy, setCreatedBy] = useState('');
  const [type, setType] = useState<IssueType | undefined>(undefined);
  const [progress, setProgress] = useState<IssueProgress | undefined>(
    undefined
  );
  const [priority, setPriority] = useState<IssuePriority | undefined>(
    undefined
  );
  const [refreshToken, setRefreshToken] = useState(0);

  const isInitialRender = useRef(true);
  const onSearchRef = useRef(onSearch);
  onSearchRef.current = onSearch;

  useEffect(() => {
    if (isInitialRender.current) {
      isInitialRender.current = false;
      return;
    }

    return delayExec(() => {
      onSearchRef.current({ id, createdBy, type, progress, priority });
    }, SEARCH_DEBOUNCE_MS);
  }, [id, createdBy, type, progress, priority, refreshToken]);

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
              value={createdBy}
              onChange={(event) => {
                setCreatedBy(event.target.value);
              }}
              label="Created By"
            />
          </Field>
          <Field>
            <IssueTypeSelect
              includeAny
              label="Type"
              value={type}
              onChange={setType}
            />
          </Field>
          <Field>
            <IssueProgressSelect
              includeAny
              label="Progress"
              value={progress}
              onChange={setProgress}
            />
          </Field>
          <Field>
            <IssuePrioritySelect
              includeAny
              label="Priority"
              value={priority}
              onChange={setPriority}
            />
          </Field>
        </Bar>
        <Box display="flex" alignItems="center">
          <TooltipActionButton
            title="Refresh"
            icon={<AutorenewOutlinedIcon />}
            onClick={() => {
              setRefreshToken((token) => token + 1);
            }}
          />
          <AddNewIssue />
        </Box>
      </Box>
    </Grow>
  );
}

export default ActionBar;
