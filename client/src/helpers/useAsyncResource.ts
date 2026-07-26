import { useCallback, useEffect, useState } from 'react';
import displayError from '@/helpers/errorHandling/displayError';

export type AsyncResource<T> = {
  data: T;
  loading: boolean;
  reload: () => void;
};

/**
 * Loads `load()` into state, cancelling the state update if the component
 * unmounts or the loader changes first. Pass a `useCallback`-stable `load` so
 * the exhaustive-deps rule can still verify what triggers a refetch.
 */
export function useAsyncResource<T>(
  load: () => Promise<T>,
  initial: T,
  fallbackMessage: string
): AsyncResource<T> {
  const [data, setData] = useState<T>(initial);
  const [loading, setLoading] = useState(true);
  const [reloadToken, setReloadToken] = useState(0);

  useEffect(() => {
    let cancelled = false;
    setLoading(true);

    load()
      .then((value) => {
        if (!cancelled) setData(value);
      })
      .catch((error: unknown) => {
        if (!cancelled) displayError(error, fallbackMessage);
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });

    return () => {
      cancelled = true;
    };
  }, [load, fallbackMessage, reloadToken]);

  const reload = useCallback(() => {
    setReloadToken((token) => token + 1);
  }, []);

  return { data, loading, reload };
}
