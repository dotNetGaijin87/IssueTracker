import { Box, Grow, TextField } from '@mui/material';
import React, { useEffect } from 'react';
import Field from '../../components/field/Field';
import Bar from '../../components/bar/Bar';
import delayExec from '../../helpers/delayExec';

interface Props {
  onSearchClicked: (value: object) => void;
}

function ActionBar({ onSearchClicked }: Props) {
  const [initRender, setInitRender] = React.useState(true);
  const [id, setName] = React.useState<string>('');
  const [email, setEmail] = React.useState<string>('');

  useEffect(() => {
    if (initRender) {
      setInitRender(false);
      return;
    }
    return delayExec(() => onSearchClicked({ id: id, email: email }), 1500);
  }, [id, email]);

  return (
    <Grow in={true}>
      <Box display="flex" alignItems="center" justifyContent="space-between">
        <Bar title="Filter">
          <Field>
            <TextField
              size="small"
              value={id}
              onChange={(e) => {
                setName(e.target.value);
              }}
              label="Name"
            />
          </Field>
          <Field>
            <TextField
              size="small"
              value={email}
              onChange={(e) => {
                setEmail(e.target.value);
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
