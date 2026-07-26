import { useId } from 'react';
import {
  FormControl,
  InputLabel,
  MenuItem,
  Select as MuiSelect
} from '@mui/material';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import StatusBadge from './StatusBadge';
import type { BadgeRegistry } from './badgeRegistry';

const ANY_VALUE = '';

export type StatusSelectProps<T extends string> = {
  value: T | undefined;
  options: readonly T[];
  registry: BadgeRegistry<T>;
  onChange: (value: T | undefined) => void;
  onBlur?: () => void;
  label?: string;
  disabled?: boolean;
  fullWidth?: boolean;
  includeAny?: boolean;
  anyLabel?: string;
};

export type BoundStatusSelectProps<T extends string> = Omit<
  StatusSelectProps<T>,
  'options' | 'registry'
>;

function StatusSelect<T extends string>({
  value,
  options,
  registry,
  onChange,
  onBlur,
  label,
  disabled = false,
  fullWidth = false,
  includeAny = false,
  anyLabel = 'N/A'
}: StatusSelectProps<T>) {
  const labelId = useId();

  return (
    <FormControl fullWidth={fullWidth} sx={{ m: '8px 8px' }}>
      {label !== undefined && <InputLabel id={labelId}>{label}</InputLabel>}
      <MuiSelect
        labelId={labelId}
        fullWidth
        size="small"
        label={label}
        disabled={disabled}
        IconComponent={KeyboardArrowDownIcon}
        value={value ?? ANY_VALUE}
        onBlur={() => onBlur?.()}
        onChange={(event) =>
          onChange(options.find((option) => option === event.target.value))
        }
      >
        {includeAny && <MenuItem value={ANY_VALUE}>{anyLabel}</MenuItem>}
        {options.map((option) => (
          <MenuItem key={option} value={option}>
            <StatusBadge value={option} registry={registry} variant="plain" />
          </MenuItem>
        ))}
      </MuiSelect>
    </FormControl>
  );
}

export default StatusSelect;
