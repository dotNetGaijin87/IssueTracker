import { FormControl, InputLabel, MenuItem, Select } from '@mui/material';
import { ProjectProgress } from '../../models/project/projectProgress';
import projectProgressList from './projectProgress/projectProgressList';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import { useState } from 'react';

interface Props {
  args?: any;
  label?: string;
  withNotApplicable?: boolean;
  disabled?: boolean;
  fullWidth?: boolean;
  defaultValue: ProjectProgress;
  onChange?: (e: any) => void;
}

function ProjectProgressSelect({
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
      if (value === ProjectProgress.Unspecified) {
        onChange(undefined);
      } else {
        onChange(value);
        console.log(value);
      }
    }
  };

  return (
    <FormControl fullWidth={fullWidth} sx={{ m: '8px 8px' }}>
      <InputLabel id="projectProgress">{label}</InputLabel>
      <Select
        fullWidth
        size="small"
        label="projectProgress"
        disabled={disabled}
        IconComponent={KeyboardArrowDownIcon}
        value={value}
        onChange={update}
        {...args}
      >
        {projectProgressList(true)
          .filter((x) =>
            withNotApplicable ? true : x.value !== ProjectProgress.Unspecified
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

export default ProjectProgressSelect;
