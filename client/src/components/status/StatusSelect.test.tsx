import { useState } from 'react';
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { describe, expect, it, vi } from 'vitest';
import { issuePriorities, IssuePriority } from '@/models/issue/issuePriority';
import { issuePriorityBadges } from '@/components/issuePriority/issuePriorityBadges';
import StatusSelect from './StatusSelect';

function Harness({
  onChange,
  includeAny = false
}: {
  onChange?: (value: IssuePriority | undefined) => void;
  includeAny?: boolean;
}) {
  const [value, setValue] = useState<IssuePriority | undefined>(
    IssuePriority.Low
  );

  return (
    <StatusSelect
      label="Priority"
      options={issuePriorities}
      registry={issuePriorityBadges}
      includeAny={includeAny}
      value={value}
      onChange={(next) => {
        setValue(next);
        onChange?.(next);
      }}
    />
  );
}

const openMenu = async (user: ReturnType<typeof userEvent.setup>) => {
  await user.click(screen.getByRole('combobox'));
};

describe('StatusSelect', () => {
  it('offers every member of the union', async () => {
    const user = userEvent.setup();
    render(<Harness />);
    await openMenu(user);

    for (const priority of issuePriorities) {
      expect(
        screen.getByRole('option', { name: priority })
      ).toBeInTheDocument();
    }
  });

  it('reports the newly selected value, not the previous one', async () => {
    // Regression: the old IssuePrioritySelect called onChange with the stale
    // state value, so picking "High" reported "Low".
    const onChange = vi.fn();
    const user = userEvent.setup();
    render(<Harness onChange={onChange} />);

    await openMenu(user);
    await user.click(screen.getByRole('option', { name: IssuePriority.High }));

    expect(onChange).toHaveBeenCalledTimes(1);
    expect(onChange).toHaveBeenCalledWith(IssuePriority.High);
  });

  it('keeps every option available after a selection', async () => {
    // Regression: the old component pruned the chosen option out of its own
    // list, permanently shrinking the dropdown.
    const user = userEvent.setup();
    render(<Harness />);

    await openMenu(user);
    await user.click(screen.getByRole('option', { name: IssuePriority.High }));
    await openMenu(user);

    for (const priority of issuePriorities) {
      expect(
        screen.getByRole('option', { name: priority })
      ).toBeInTheDocument();
    }
  });

  it('maps the "any" entry to undefined for filter use', async () => {
    const onChange = vi.fn();
    const user = userEvent.setup();
    render(<Harness onChange={onChange} includeAny />);

    await openMenu(user);
    await user.click(screen.getByRole('option', { name: 'N/A' }));

    expect(onChange).toHaveBeenCalledWith(undefined);
  });

  it('omits the "any" entry unless asked for', async () => {
    const user = userEvent.setup();
    render(<Harness />);
    await openMenu(user);

    expect(
      screen.queryByRole('option', { name: 'N/A' })
    ).not.toBeInTheDocument();
  });
});
