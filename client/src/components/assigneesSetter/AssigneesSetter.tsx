import * as React from 'react';
import {
  Autocomplete as MuiAutocomplete,
  Avatar,
  AvatarGroup,
  Checkbox,
  Chip,
  TextField,
  CircularProgress,
  InputAdornment,
  MenuItem
} from '@mui/material';
import CheckBoxOutlineBlankIcon from '@mui/icons-material/CheckBoxOutlineBlank';
import CheckIcon from '@mui/icons-material/Check';
import { adapter } from '../../adapters/adapter';
import delayExec from '../../helpers/delayExec';
import { IssuePermission } from '../../models/issue/issuePermission';

const icon = <CheckBoxOutlineBlankIcon fontSize="small" />;
const checkedIcon = <CheckIcon fontSize="small" />;

interface Props {
  issueId?: string;
  disabled?: boolean;
  onChange: (selected: string[]) => void;
}

export default function AssigneesSetter({
  issueId,
  disabled,
  onChange
}: Props) {
  const [open, setOpen] = React.useState(false);
  const [search, setSearch] = React.useState('');
  const [options, setOptions] = React.useState<string[]>([]);
  const [usersWithPermissions, setUsersWithPermissions] = React.useState<
    string[]
  >([]);
  const [loading, setLoading] = React.useState(false);
  const isNewIssue = issueId ? false : true;

  React.useEffect(() => {
    return delayExec(async () => {
      setLoading(true);
      let allUsers: string[] = [];
      let usersWithPermissions: string[] = [];
      let searchedUsers: string[] = [];
      if (!isNewIssue) {
        let userPermissionList = await adapter.Permission.list({ issueId });
        usersWithPermissions = userPermissionList.permissions.map(
          (x: any) => x.userId
        );
      }
      if (!disabled) {
        searchedUsers = (await adapter.User.list({ id: search })).users.map(
          (x: any) => x.id
        );
      }

      allUsers = usersWithPermissions.concat(
        searchedUsers.filter(
          (item: string) => usersWithPermissions.indexOf(item) < 0
        )
      );

      setOptions(allUsers);
      setUsersWithPermissions(usersWithPermissions);
      onChange(usersWithPermissions);
      setLoading(false);
    }, 1500);
  }, [issueId, search]);

  const updateUserIssueRelationship = async (
    e: any,
    option: any,
    reason: any
  ) => {
    if (reason === 'selectOption') {
      if (!isNewIssue) {
        await adapter.Permission.create({
          issueId: issueId,
          userId: option[option.length - 1],
          issuePermission: IssuePermission.CanModify,
          isPinnedToKanban: true
        });
      }
      let newUsersWithClaim = [
        ...usersWithPermissions,
        option[option.length - 1]
      ];
      setUsersWithPermissions(newUsersWithClaim);
      onChange(newUsersWithClaim);
    } else if (reason === 'removeOption') {
      let removedUser = usersWithPermissions.filter(
        (x) => !option.includes(x)
      )[0];
      if (!isNewIssue) {
        await adapter.Permission.delete(removedUser, issueId!);
      }
      setUsersWithPermissions(option);
      onChange(option);
    }
  };

  return loading ? (
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
  ) : (
    <MuiAutocomplete
      sx={{ m: '8px 8px', width: 'inherit' }}
      multiple
      disableCloseOnSelect
      open={open}
      value={usersWithPermissions}
      onChange={(value, option, reason) => {
        if (!disabled) {
          updateUserIssueRelationship(value, option, reason);
        }
      }}
      onOpen={() => {
        setOpen(true);
      }}
      onClose={() => {
        setOpen(false);
      }}
      getOptionLabel={(option) => option}
      options={options}
      popupIcon={<></>}
      clearIcon={<></>}
      renderTags={(value, getTagProps) => (
        <AvatarGroup max={3} {...getTagProps}>
          {value.map((option, index) => (
            <Avatar key={option} sx={{ width: '24px', height: '24px' }}>
              {option.substring(0, 2)}
            </Avatar>
          ))}
        </AvatarGroup>
      )}
      renderOption={(props, option, { selected }) => (
        <MenuItem key={option} {...props} sx={{ m: 0, p: 0 }}>
          <>
            <Checkbox
              icon={icon}
              checkedIcon={checkedIcon}
              checked={selected}
            />
            <Chip
              size="small"
              label={option}
              avatar={<Avatar>{option.substring(0, 2)}</Avatar>}
            />
          </>
        </MenuItem>
      )}
      renderInput={(params) => (
        <TextField
          sx={{ margin: 0, widht: 'inherit' }}
          {...params}
          InputProps={{
            ...params.InputProps
          }}
          disabled={disabled}
          size="small"
          onChange={(e) => {
            setSearch(e.target.value);
          }}
        />
      )}
    />
  );
}
