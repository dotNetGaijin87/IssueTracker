import { createTheme } from '@mui/material';

const darkBackgroundDefaultColor = '#0d1117';
const darkBackgroundPaperColor = '#161b22';
const darkElevatedColor = '#1c2230';
const darkDivider = 'rgba(255,255,255,0.08)';
const darkTextPrimary = '#e6e8eb';
const darkTextSecondary = '#8b94a3';

const lightBackgroundDefaultColor = '#f8fafc';
const lightBackgroundPaperColor = '#ffffff';
const lightDivider = 'rgba(15,23,42,0.08)';
const lightTextPrimary = '#0f172a';
const lightTextSecondary = '#475569';

const indigoMain = '#6366f1';
const indigoLight = '#818cf8';
const indigoDark = '#4f46e5';
const skyMain = '#38bdf8';
const removeColor = '#f87171';

const getColor = (
  palleType: 'light' | 'dark',
  ligthColor: string,
  darkColor: string
) => {
  if (palleType === 'light') return ligthColor;
  else return darkColor;
};

function getTheme(palleteType: 'light' | 'dark') {
  document.documentElement.setAttribute('data-color-mode', palleteType);

  const isLight = palleteType === 'light';
  const paper = isLight ? lightBackgroundPaperColor : darkBackgroundPaperColor;
  const divider = isLight ? lightDivider : darkDivider;
  const elevated = isLight ? lightBackgroundPaperColor : darkElevatedColor;
  const inputBg = getColor(
    palleteType,
    lightBackgroundDefaultColor,
    darkBackgroundDefaultColor
  );

  return createTheme({
    palette: {
      mode: palleteType,
      primary: {
        main: indigoMain,
        light: indigoLight,
        dark: indigoDark,
        contrastText: '#ffffff'
      },
      secondary: {
        main: skyMain,
        light: '#7dd3fc',
        dark: '#0ea5e9',
        contrastText: '#0b1220'
      },
      success: { main: '#34d399' },
      warning: { main: '#fbbf24' },
      info: { main: '#60a5fa' },
      error: { main: removeColor, dark: '#7f1d1d' },
      divider,
      ...(isLight
        ? {
            text: { primary: lightTextPrimary, secondary: lightTextSecondary },
            background: {
              default: lightBackgroundDefaultColor,
              paper: lightBackgroundPaperColor
            }
          }
        : {
            text: { primary: darkTextPrimary, secondary: darkTextSecondary },
            background: {
              default: darkBackgroundDefaultColor,
              paper: darkBackgroundPaperColor
            }
          })
    },
    shape: { borderRadius: 10 },
    typography: {
      fontFamily:
        '"Inter", system-ui, "Segoe UI", Roboto, Helvetica, Arial, sans-serif',
      h1: { fontWeight: 700, letterSpacing: '-0.02em' },
      h2: { fontWeight: 700, letterSpacing: '-0.02em' },
      h3: { fontWeight: 700, letterSpacing: '-0.015em' },
      h4: { fontWeight: 600, letterSpacing: '-0.01em' },
      h5: { fontWeight: 600 },
      h6: { fontWeight: 600 },
      subtitle1: { fontWeight: 500 },
      subtitle2: { fontWeight: 500 },
      button: { fontWeight: 600, textTransform: 'none', letterSpacing: 0 }
    },
    components: {
      MuiCssBaseline: {
        styleOverrides: {
          '*::-webkit-scrollbar': { width: 8, height: 8 },
          '*::-webkit-scrollbar-track': { backgroundColor: 'transparent' },
          '*::-webkit-scrollbar-thumb': {
            backgroundColor: getColor(
              palleteType,
              'rgba(15,23,42,0.18)',
              'rgba(255,255,255,0.14)'
            ),
            borderRadius: 8
          },
          '*::-webkit-scrollbar-thumb:hover': {
            backgroundColor: getColor(
              palleteType,
              'rgba(15,23,42,0.30)',
              'rgba(255,255,255,0.24)'
            )
          }
        }
      },
      MuiPaper: {
        styleOverrides: {
          root: { backgroundImage: 'none' }
        }
      },
      MuiButton: {
        defaultProps: { disableElevation: true },
        styleOverrides: {
          root: {
            textTransform: 'none',
            fontWeight: 600,
            minWidth: 110,
            height: 40,
            margin: '5px',
            borderRadius: 8,
            transition:
              'transform .16s ease, box-shadow .16s ease, background-color .16s ease'
          },
          contained: {
            '&:hover': {
              transform: 'translateY(-1px)',
              boxShadow: `0 6px 18px ${indigoMain}40`
            }
          }
        }
      },
      MuiSelect: {
        styleOverrides: {
          select: { backgroundColor: inputBg }
        }
      },
      MuiAutocomplete: {
        styleOverrides: {
          listbox: {
            backgroundColor: elevated,
            '& .MuiAutocomplete-option': {
              backgroundColor: 'transparent',
              borderRadius: 6
            }
          },
          paper: {
            border: `1px solid ${divider}`,
            borderRadius: 12,
            boxShadow: '0 12px 32px rgba(0,0,0,0.35)'
          }
        }
      },
      MuiTextField: {
        styleOverrides: {
          root: {
            margin: '8px 8px',
            boxSizing: 'border-box',
            backgroundColor: inputBg,
            borderRadius: 8
          }
        }
      },
      MuiOutlinedInput: {
        styleOverrides: {
          root: {
            borderRadius: 8,
            transition: 'box-shadow .16s ease',
            '&.Mui-focused': { boxShadow: `0 0 0 2px ${indigoMain}55` }
          },
          notchedOutline: { border: 'none' }
        }
      },
      MuiInputBase: { styleOverrides: { input: { textAlign: 'left' } } },
      MuiChip: {
        styleOverrides: {
          root: { borderRadius: 8, fontWeight: 500 },
          deleteIcon: {
            color: removeColor,
            '&:hover': { color: removeColor }
          }
        }
      },
      MuiTooltip: {
        styleOverrides: {
          tooltip: {
            backgroundColor: elevated,
            border: `1px solid ${divider}`,
            borderRadius: 8,
            fontSize: 12,
            color: getColor(palleteType, lightTextPrimary, darkTextPrimary),
            boxShadow: '0 8px 24px rgba(0,0,0,0.35)'
          }
        }
      },
      MuiDialog: {
        styleOverrides: {
          paper: {
            minWidth: '800px',
            borderRadius: 16,
            border: `1px solid ${divider}`,
            boxShadow: '0 24px 64px rgba(0,0,0,0.5)'
          }
        }
      },
      MuiMenu: {
        styleOverrides: {
          paper: {
            border: `1px solid ${divider}`,
            borderRadius: 12,
            boxShadow: '0 12px 32px rgba(0,0,0,0.35)'
          },
          list: { backgroundColor: elevated }
        }
      },
      MuiTab: {
        styleOverrides: {
          root: {
            minHeight: 20,
            borderRadius: 8,
            textTransform: 'none',
            fontWeight: 500,
            '&.Mui-selected': {
              backgroundColor: paper,
              borderColor: divider,
              borderStyle: 'solid',
              borderWidth: '1px'
            }
          }
        }
      },
      MuiTableCell: {
        styleOverrides: {
          root: { borderColor: 'transparent', textAlign: 'center' }
        }
      },
      MuiDrawer: {
        styleOverrides: {
          paper: { border: 'none' }
        }
      }
    }
  });
}

export default getTheme;
