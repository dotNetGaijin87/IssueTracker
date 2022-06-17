import { TabList as MuiTabList } from '@mui/lab';

interface Props {
  children: React.ReactNode;
  onChange: (event: any, newValue: string) => void;
}

function TabList({ onChange, children }: Props) {
  return (
    <MuiTabList
      onChange={(event: any, newValue: string) => {
        onChange(event, newValue);
      }}
      textColor="secondary"
      sx={{
        ml: '20px',
        boxSizing: 'border-box',
        bgcolor: '#00000039',
        borderRadius: 1,
        padding: 0.5,
        width: 'fit-content',
        borderColor: 'divider',
        borderWidth: '1px',
        borderStyle: 'inset'
      }}
      TabIndicatorProps={{
        style: {
          display: 'none'
        }
      }}
    >
      {children}
    </MuiTabList>
  );
}

export default TabList;
