function parseDateTimeToMessage(date: Date | null | undefined): string {
  if (!date) return '';
  const diffTime = (new Date().getTime() - new Date(date).getTime()) / 1000;
  let seconds = Math.ceil(diffTime);
  if (seconds < 60) return `${seconds} seconds ago`;
  let minutes = Math.ceil(seconds / 60);
  if (minutes < 60) return `${minutes} minutes ago`;
  let hours = Math.ceil(minutes / 60);
  if (hours < 24) return `${hours} hours ago`;
  let days = Math.ceil(hours / 24);
  if (days < 7) return `${days} days ago`;

  return new Date(date).toLocaleString();
}

export default parseDateTimeToMessage;
