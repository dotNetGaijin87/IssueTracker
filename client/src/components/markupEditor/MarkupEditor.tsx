import { Box, Typography } from '@mui/material';
import MDEditor from '@uiw/react-md-editor';
import rehypeSanitize from 'rehype-sanitize';

interface Props {
  title: string;
  value: string;
  disabled?: boolean;
  onBlur: () => void;
  onChange: () => void;
}

function MarkupEditor({ title, value, disabled, onBlur, onChange }: Props) {
  return (
    <Box
      sx={{
        p: 1
      }}
    >
      <Typography sx={{ mb: 1 }}>{title}</Typography>
      <Box
        className="wmde-markdown-var"
        sx={{
          bgcolor: 'background.default',
          borderRadius: 2,
          overflowY: 'hidden',
          borderStyle: 'solid',
          borderWidth: '1px',
          borderColor: '#00000050',
          height: 'auto',
          mb: 1,
          display: 'flex'
        }}
        data-color-mode="dark"
      >
        <Box sx={{ flexGrow: 1 }}>
          {disabled ? (
            <MDEditor
              value={value}
              onBlur={onBlur}
              previewOptions={{
                rehypePlugins: [[rehypeSanitize]]
              }}
            />
          ) : (
            <MDEditor
              value={value}
              onBlur={onBlur}
              onChange={onChange}
              previewOptions={{
                rehypePlugins: [[rehypeSanitize]]
              }}
            />
          )}
        </Box>
      </Box>
    </Box>
  );
}

export default MarkupEditor;
