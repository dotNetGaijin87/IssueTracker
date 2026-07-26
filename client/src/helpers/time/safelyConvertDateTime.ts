function safelyConvertDateTime(date: Date | null | undefined): string {
  if (!date) return '';

  const value = new Date(date);
  return Number.isNaN(value.getTime()) ? '' : value.toLocaleString();
}

export default safelyConvertDateTime;
