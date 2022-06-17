import React, { useEffect } from 'react';
import { Box, Grow, TextField } from '@mui/material';
import AutorenewOutlinedIcon from '@mui/icons-material/AutorenewOutlined';
import AddNewIssue from './AddNewIssue';
import IssueProgressSelect from '../../../components/issueProgress/IssueProgressSelect';
import IssuePrioritySelect from '../../../components/issuePriority/IssuePrioritySelect';
import Field from '../../../components/field/Field';
import Bar from '../../../components/bar/Bar';
import delayExec from '../../../helpers/delayExec';
import TooltipActionButton from '../../../components/tooltipActionButton/TooltipActionButton';
import { IssueType } from '../../../models/issue/issueType';
import IssueTypeSelect from '../../../components/issueType/IssueTypeSelect';
import { IssueProgress } from '../../../models/issue/issueProgress';
import { IssuePriority } from '../../../models/issue/issuePriority';

interface Props {
  onSearch: (value: IssueListSearchCriteria) => void;
}

interface IssueListSearchCriteria {
  name: '';
  createdBy: '';
  Type: IssueType;
  Progress: IssueProgress;
  Priority: IssuePriority;
}

function ActionBar({ onSearch }: Props) {
  const [initRender, setInitRender] = React.useState(true);
  const [name, setName] = React.useState('');
  const [createdBy, setCreatedBy] = React.useState('');
  const [issueType, setIssueType] = React.useState(IssueType.Unspecified);
  const [issueProgress, setIssueProgress] = React.useState(
    IssueProgress.Unspecified
  );
  const [issuePriority, setIssuePriority] = React.useState(
    IssuePriority.Unspecified
  );

  const [updateData, setUpdateData] = React.useState(false);

  useEffect(() => {
    if (initRender) {
      setInitRender(false);
      return;
    }
    setUpdateData(false);
    let search: any = {
      name: name,
      createdBy: createdBy,
      Type: issueType === IssueType.Unspecified ? undefined : issueType,
      Progress:
        issueProgress === IssueProgress.Unspecified ? undefined : issueProgress,
      Priority:
        issuePriority === IssuePriority.Unspecified ? undefined : issuePriority
    };
    return delayExec(() => onSearch({ ...search }), 1500);
  }, [name, createdBy, issuePriority, issueProgress, issueType, updateData]);

  return (
    <Grow in={true}>
      <Box display="flex" alignItems="center" justifyContent="space-between">
        <Bar title="Filter">
          <Field>
            <TextField
              size="small"
              value={name}
              onChange={(e) => {
                setName(e.target.value);
              }}
              label="Name"
            />
          </Field>
          <Field>
            <TextField
              size="small"
              value={createdBy}
              onChange={(e) => {
                setCreatedBy(e.target.value);
              }}
              label="Created By"
            />
          </Field>
          <Field>
            <IssueTypeSelect
              withNotApplicable={true}
              defaultValue={IssueType.Unspecified}
              label="Type"
              onChange={setIssueType}
            />
          </Field>
          <Field>
            <IssueProgressSelect
              withNotApplicable={true}
              defaultValue={IssueProgress.Unspecified}
              label="Progress"
              onChange={setIssueProgress}
            />
          </Field>
          <Field>
            <IssuePrioritySelect
              withNotApplicable={true}
              defaultValue={IssuePriority.Unspecified}
              label="Priority"
              onChange={setIssuePriority}
            />
          </Field>
        </Bar>
        <Box display="flex" alignItems="center">
          <TooltipActionButton
            title={'Refresh'}
            icon={<AutorenewOutlinedIcon />}
            onClick={() => setUpdateData(true)}
          />
          <AddNewIssue />
        </Box>
      </Box>
    </Grow>
  );
}

export default ActionBar;
export type { IssueListSearchCriteria };
