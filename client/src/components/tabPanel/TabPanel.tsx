import { TabPanel as MuiTabPanel } from '@mui/lab';

interface Props {
  children: React.ReactNode;
  value: string;
}

function TabPanel({ value, children }: Props) {
  return (
    <MuiTabPanel
      value={value}
      sx={{
        borderRadius: 2,
        height: '100%',
        boxSizing: 'border-box'
      }}
    >
      {children}
    </MuiTabPanel>
  );
}

export default TabPanel;
