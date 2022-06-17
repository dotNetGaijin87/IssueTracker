import { createTheme } from '@mui/material';

const darkBackgorundDefaultColor = '#24292d';
const darkBackgorundPaperColor = '#1e2225';

const darkSecondaryMainColor = '#24abd5';
const darkRemoveColor = 'tomato';

const lightBackgorundDefaultColor = '#F3F6F9';
const lightBackgorundPaperColor = '#fff';
const lightSecondaryMainColor = '#24abd5';
const lightTextPrimary = '#132F4C';

const getColor = (
  palleType: 'light' | 'dark',
  ligthColor: string,
  darkColor: string
) => {
  if (palleType === 'light') return ligthColor;
  else return darkColor;
};

function getTheme(palleteType: 'light' | 'dark') {
  if (palleteType === 'light') {
    document.documentElement.setAttribute('data-color-mode', 'light');
  } else {
    document.documentElement.setAttribute('data-color-mode', 'dark');
  }

  return createTheme({
    palette: {
      mode: palleteType,
      ...(palleteType === 'light'
        ? {
            text: {
              primary: `${lightTextPrimary}`
            },
            background: {
              default: `${lightBackgorundDefaultColor}`,
              paper: `${lightBackgorundPaperColor}`
            },
            primary: {
              main: '#45444b',
              light: '#e9e0df',
              dark: '#83858a'
            },
            secondary: {
              main: `${lightSecondaryMainColor}`,
              light: '#e9e0df',
              dark: '#83858a'
            }
          }
        : {
            text: {
              primary: '#e0e0e3',
              secondary: '#a69f9f'
            },
            background: {
              default: `${darkBackgorundDefaultColor}`,
              paper: `${darkBackgorundPaperColor}`
            },
            primary: {
              main: '#9Db4BF',
              light: '#494a4c',
              dark: '#3e3f41'
            },
            secondary: {
              main: `${darkSecondaryMainColor}`,
              light: '#a3501e',
              dark: '#ffc68a'
            },
            error: { main: '#FF6347', dark: '#911e1e42' }
          })
    },
    components: {
      MuiPaper: {
        defaultProps: {},
        styleOverrides: {
          root: {
            boxShadow: 'none',
            backgroundImage: 'none'
          }
        }
      },
      MuiSelect: {
        styleOverrides: {
          select: {
            backgroundColor: `${getColor(
              palleteType,
              lightBackgorundDefaultColor,
              darkBackgorundDefaultColor
            )}`,
            root: {
              paper: {
                borderRadius: '1px'
              }
            }
          }
        }
      },
      MuiButton: {
        styleOverrides: { root: { width: 120, height: 40, margin: '5px' } }
      },
      MuiAutocomplete: {
        styleOverrides: {
          listbox: {
            backgroundColor: `${getColor(
              palleteType,
              lightBackgorundDefaultColor,
              darkBackgorundDefaultColor
            )}`,
            '& .MuiAutocomplete-option': {
              backgroundColor: `transparent`
            }
          }
        }
      },
      MuiIconButton: {
        styleOverrides: {}
      },
      MuiTextField: {
        styleOverrides: {
          root: {
            margin: '8px 8px',
            boxSizing: 'border-box',
            backgroundColor: `${getColor(
              palleteType,
              lightBackgorundDefaultColor,
              darkBackgorundDefaultColor
            )}`,
            borderRadius: '5px'
          }
        }
      },
      MuiChip: {
        styleOverrides: {
          deleteIcon: {
            color: `${darkRemoveColor}`,
            '&:hover': { color: `${darkRemoveColor}` }
          }
        }
      },
      MuiTooltip: {
        styleOverrides: {
          tooltip: {
            backgroundColor: `${getColor(
              palleteType,
              lightBackgorundPaperColor,
              darkBackgorundPaperColor
            )}`,
            border: '1px solid #00000025',
            color: `${getColor(
              palleteType,
              lightSecondaryMainColor,
              darkSecondaryMainColor
            )}`
          }
        }
      },
      MuiDialog: { styleOverrides: { paper: { minWidth: '800px' } } },

      MuiTab: {
        styleOverrides: {
          root: {
            minHeight: 20,
            borderRadius: '6px',
            '&.Mui-selected': {
              backgroundColor: `${getColor(
                palleteType,
                lightBackgorundPaperColor,
                darkBackgorundPaperColor
              )}`,

              borderColor: `#ffffff16`,
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
      MuiMenu: {
        styleOverrides: {
          list: {
            backgroundColor: `${getColor(
              palleteType,
              lightBackgorundDefaultColor,
              darkBackgorundDefaultColor
            )}`
          }
        }
      },

      MuiInputBase: { styleOverrides: { input: { textAlign: 'left' } } },
      MuiOutlinedInput: {
        styleOverrides: {
          notchedOutline: { border: 'none' }
        }
      },
      MuiDrawer: {
        styleOverrides: {
          root: {},
          paper: {
            border: 'none'
          }
        }
      },
      MuiSvgIcon: {
        styleOverrides: {
          root: {}
        }
      }
    }
  });
}

export default getTheme;
