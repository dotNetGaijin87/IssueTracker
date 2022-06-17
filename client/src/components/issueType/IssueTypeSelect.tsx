import { useState } from 'react';
import { FormControl, InputLabel, MenuItem, Select } from '@mui/material';
import issueTypeList from './issueType/issueTypeList';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import { IssueType } from '../../models/issue/issueType';

interface Props {
  args?: any;
  label?: string;
  withNotApplicable?: boolean;
  disabled?: boolean;
  fullWidth?: boolean;
  defaultValue: IssueType;
  onChange?: (e: any) => void;
}

function IssueTypeSelect({
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
    setValue(value);
    if (onChange) {
      if (value === IssueType.Unspecified) {
        onChange(undefined);
      } else {
        onChange(value);
      }
    }
  };

  return (
    <FormControl fullWidth={fullWidth} sx={{ m: '8px 8px' }}>
      <InputLabel id="issueType">{label}</InputLabel>
      <Select
        fullWidth
        size="small"
        label="issueType"
        disabled={disabled}
        IconComponent={KeyboardArrowDownIcon}
        value={value}
        onChange={update}
        {...args}
      >
        {issueTypeList(true)
          .filter((x) =>
            withNotApplicable ? true : x.value !== IssueType.Unspecified
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

export default IssueTypeSelect;
