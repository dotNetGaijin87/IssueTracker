import { render, screen } from '@testing-library/react';
import { describe, expect, it } from 'vitest';
import { issueProgressBadges } from '@/components/issueProgress/issueProgressBadges';
import { issueProgresses, IssueProgress } from '@/models/issue/issueProgress';
import StatusBadge from './StatusBadge';

describe('StatusBadge', () => {
  it('renders the label for every member of the union', () => {
    for (const progress of issueProgresses) {
      const { unmount } = render(
        <StatusBadge value={progress} registry={issueProgressBadges} />
      );
      expect(screen.getByText(progress)).toBeInTheDocument();
      unmount();
    }
  });

  it('renders nothing when there is no value', () => {
    const { container } = render(
      <StatusBadge value={undefined} registry={issueProgressBadges} />
    );
    expect(container).toBeEmptyDOMElement();
  });

  it('applies the registry colour in the text variant', () => {
    render(
      <StatusBadge
        value={IssueProgress.Canceled}
        registry={issueProgressBadges}
        variant="text"
      />
    );
    expect(screen.getByText(IssueProgress.Canceled)).toHaveStyle({
      color: issueProgressBadges[IssueProgress.Canceled].color
    });
  });
});
