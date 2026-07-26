import { Box, Typography } from '@mui/material';
import type { BadgeSpec, BadgeVariant } from './badgeRegistry';

interface Props {
  label: string;
  spec: BadgeSpec;
  variant: BadgeVariant;
}

function BaseBadge({ label, spec, variant }: Props): JSX.Element {
  switch (variant) {
    case 'plain':
      return (
        <Box component="span">
          <Typography>{label}</Typography>
        </Box>
      );
    case 'text':
      return (
        <Box component="span">
          <Typography sx={{ color: spec.color, bgcolor: 'transparent' }}>
            {label}
          </Typography>
        </Box>
      );
    case 'chip':
      return (
        <Box display="flex" justifyContent="center">
          <Box
            sx={{
              boxSizing: 'border-box',
              minWidth: '120px',
              height: '30px',
              borderRadius: 4,
              margin: 1,
              p: 1,
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              color: spec.color,
              backgroundColor: spec.backgroundColor
            }}
          >
            <Typography>{label}</Typography>
          </Box>
        </Box>
      );
  }
}

export default BaseBadge;
