import { MenuItem, TextField } from '@mui/material';

interface Props<T> {
  elements: T;
  initialItem: unknown;
}

function Select<T extends object>({ elements, initialItem }: Props<T>) {
  return (
    <TextField
      size="small"
      select
      variant="standard"
      sx={{
        width: '100px'
      }}
      InputProps={{ disableUnderline: true }}
      defaultValue={initialItem}
    >
      {Object.entries(elements)
        .filter((element) => {
          return isNaN(Number(element[0]));
        })
        .map((element) => (
          <MenuItem value={element[0]} key={element[0]}>
            {element[1]}
          </MenuItem>
        ))}
    </TextField>
  );
}

export default Select;
