import { FormControl, InputLabel, MenuItem, Select } from '@mui/material';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import issueProgressList from './issueProgress/issueProgressList';
import { IssueProgress } from '../../models/issue/issueProgress';
import { useState } from 'react';

interface Props {
  args?: any;
  label?: string;
  withNotApplicable?: boolean;
  disabled?: boolean;
  fullWidth?: boolean;
  defaultValue: IssueProgress;
  onChange?: (e: any) => void;
}

function IssueProgressSelect({
  args,
  label,
  withNotApplicable = false,
  disabled,
  fullWidth,
  defaultValue,
  onChange
}: Props) {
  const [value, setValue] = useState(defaultValue);

  const update = (e: any) => {
    let value = e.target.value;
    setValue(e.target.value);
    if (onChange) {
      if (value === IssueProgress.Unspecified) {
        onChange(undefined);
      } else {
        onChange(value);
      }
    }
  };

  return (
    <FormControl fullWidth={fullWidth} sx={{ m: '8px 8px' }}>
      <InputLabel id="issueProgress">{label}</InputLabel>
      <Select
        fullWidth
        size="small"
        label="issueProgress"
        disabled={disabled}
        IconComponent={KeyboardArrowDownIcon}
        value={value}
        onChange={update}
        {...args}
      >
        {issueProgressList(true)
          .filter((x) =>
            withNotApplicable ? true : x.value !== IssueProgress.Unspecified
          )
          .map((item) => (
            <MenuItem key={item.value} value={item.value}>
              {item.element}
            </MenuItem>
          ))}
      </Select>
    </FormControl>
  );
}

export default IssueProgressSelect;
