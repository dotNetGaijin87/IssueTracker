/** Runs `action` after `delayMs` and returns a cancel function for cleanup. */
function delayExec(action: () => void, delayMs: number): () => void {
  const timeoutId = setTimeout(action, delayMs);
  return () => {
    clearTimeout(timeoutId);
  };
}

export default delayExec;
