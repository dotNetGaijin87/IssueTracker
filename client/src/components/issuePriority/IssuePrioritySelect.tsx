import { FormControl, InputLabel, MenuItem, Select } from '@mui/material';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import issuePriorityList from './issuePriority/issuePriorityList';
import { IssuePriority } from '../../models/issue/issuePriority';
import { useState } from 'react';

interface Props {
  args?: any;
  label?: string;
  withNotApplicable?: boolean;
  disabled?: boolean;
  fullWidth?: boolean;
  defaultValue: IssuePriority;
  onChange?: (e: any) => void;
}

function IssuePrioritySelect({
  args,
  label,
  withNotApplicable = false,
  disabled,
  fullWidth,
  defaultValue,
  onChange
}: Props) {
  const [value, setValue] = useState(defaultValue);
  const [options, setOptions] = useState(
    issuePriorityList(true, false).filter((x) =>
      withNotApplicable ? true : x.value !== IssuePriority.Unspecified
    )
  );

  const update = (e: any) => {
    let newValue = e.target.value;
    setValue(newValue);
    if (onChange) {
      if (value === IssuePriority.Unspecified) {
        onChange(undefined);
      } else {
        onChange(value);
      }
    }
    setOptions(options.filter((x) => x.value !== value));
  };

  return (
    <FormControl fullWidth={fullWidth} sx={{ m: '8px 8px' }}>
      <InputLabel id="issuePriority">{label}</InputLabel>
      <Select
        fullWidth
        size="small"
        label="issuePriority"
        disabled={disabled}
        IconComponent={KeyboardArrowDownIcon}
        value={value}
        onChange={update}
        {...args}
      >
        {options.map((item) => (
          <MenuItem key={item.value} value={item.value}>
            {item.element}
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
}

export default IssuePrioritySelect;
