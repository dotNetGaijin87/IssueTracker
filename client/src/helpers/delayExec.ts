function delayExec(fun: () => void, delayTime: number) {
  const timeOutId = setTimeout(() => fun(), delayTime);
  return () => clearTimeout(timeOutId);
}

export default delayExec;
