import {
  useCallback,
  useEffect,
  useRef,
  useState,
  type HTMLAttributes,
  type Key
} from 'react';
import {
  Autocomplete as MuiAutocomplete,
  Avatar,
  AvatarGroup,
  Checkbox,
  Chip,
  CircularProgress,
  InputAdornment,
  TextField,
  type TextFieldProps
} from '@mui/material';
import CheckBoxOutlineBlankIcon from '@mui/icons-material/CheckBoxOutlineBlank';
import CheckIcon from '@mui/icons-material/Check';
import { adapter } from '@/adapters/adapter';
import delayExec from '@/helpers/delayExec';
import displayError from '@/helpers/errorHandling/displayError';
import type { IssueId, UserId } from '@/models/ids';
import { IssuePermission } from '@/models/issue/issuePermission';

const SEARCH_DEBOUNCE_MS = 1500;
const AVATAR_INITIALS = 2;

const unchecked = <CheckBoxOutlineBlankIcon fontSize="small" />;
const checked = <CheckIcon fontSize="small" />;

type OptionProps = HTMLAttributes<HTMLLIElement> & { key?: Key };

interface Props {
  issueId?: IssueId | undefined;
  disabled?: boolean;
  onChange?: (selected: UserId[]) => void;
}

function AssigneesSetter({ issueId, disabled = false, onChange }: Props) {
  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState('');
  const [options, setOptions] = useState<UserId[]>([]);
  const [assignees, setAssignees] = useState<UserId[]>([]);
  const [loading, setLoading] = useState(false);

  const onChangeRef = useRef(onChange);
  onChangeRef.current = onChange;

  const publish = useCallback((selected: UserId[]) => {
    setAssignees(selected);
    onChangeRef.current?.(selected);
  }, []);

  useEffect(() => {
    let cancelled = false;

    const loadCandidates = async () => {
      setLoading(true);
      try {
        const assigned =
          issueId === undefined
            ? []
            : (await adapter.Permission.list({ issueId })).items.map(
                (permission) => permission.userId
              );

        const searched = disabled
          ? []
          : (await adapter.User.list({ id: search })).items.map(
              (user) => user.id
            );

        if (cancelled) return;

        setOptions([
          ...assigned,
          ...searched.filter((user) => !assigned.includes(user))
        ]);
        publish(assigned);
      } catch (error) {
        if (!cancelled) displayError(error, 'Loading assignees failed');
      } finally {
        if (!cancelled) setLoading(false);
      }
    };

    const cancelTimer = delayExec(() => {
      void loadCandidates();
    }, SEARCH_DEBOUNCE_MS);

    return () => {
      cancelled = true;
      cancelTimer();
    };
  }, [issueId, search, disabled, publish]);

  const handleAssigneesChange = async (selected: readonly UserId[]) => {
    const next = [...selected];

    try {
      const added = next.find((user) => !assignees.includes(user));
      const removed = assignees.find((user) => !next.includes(user));

      if (issueId !== undefined && added !== undefined) {
        await adapter.Permission.create({
          issueId,
          userId: added,
          issuePermission: IssuePermission.CanModify,
          isPinnedToKanban: true
        });
      }

      if (issueId !== undefined && removed !== undefined) {
        await adapter.Permission.delete(removed, issueId);
      }

      publish(next);
    } catch (error) {
      displayError(error, 'Updating assignees failed');
    }
  };

  if (loading) {
    return (
      <TextField
        sx={{ m: '8px 8px', width: 'inherit' }}
        size="small"
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <CircularProgress size="24px" />
            </InputAdornment>
          )
        }}
      />
    );
  }

  return (
    <MuiAutocomplete
      sx={{ m: '8px 8px', width: 'inherit' }}
      multiple
      disableCloseOnSelect
      open={open}
      value={assignees}
      options={options}
      onOpen={() => {
        setOpen(true);
      }}
      onClose={() => {
        setOpen(false);
      }}
      onChange={(_event, selected) => {
        if (!disabled) void handleAssigneesChange(selected);
      }}
      getOptionLabel={(option) => option}
      popupIcon={null}
      clearIcon={null}
      renderTags={(value) => (
        <AvatarGroup max={3}>
          {value.map((option) => (
            <Avatar key={option} sx={{ width: '24px', height: '24px' }}>
              {option.substring(0, AVATAR_INITIALS)}
            </Avatar>
          ))}
        </AvatarGroup>
      )}
      renderOption={(props, option, { selected }) => {
        // MUI types this parameter's `key` as `any`; the assertion narrows it.
        const { key, ...optionProps } = props as OptionProps;
        return (
          <li key={key} {...optionProps} style={{ margin: 0, padding: 0 }}>
            <Checkbox
              icon={unchecked}
              checkedIcon={checked}
              checked={selected}
            />
            <Chip
              size="small"
              label={option}
              avatar={<Avatar>{option.substring(0, AVATAR_INITIALS)}</Avatar>}
            />
          </li>
        );
      }}
      renderInput={(params) => (
        <TextField
          // MUI types its own `AutocompleteRenderInputParams` with optional
          // props that `exactOptionalPropertyTypes` rejects on TextField.
          {...(params as TextFieldProps)}
          sx={{ margin: 0, width: 'inherit' }}
          disabled={disabled}
          size="small"
          onChange={(event) => {
            setSearch(event.target.value);
          }}
        />
      )}
    />
  );
}

export default AssigneesSetter;
