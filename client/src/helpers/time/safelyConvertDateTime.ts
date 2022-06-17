function safelyConvertDateTime(date: Date | null | undefined): string {
  return date ? new Date(date).toLocaleString() : '';
}

export default safelyConvertDateTime;
