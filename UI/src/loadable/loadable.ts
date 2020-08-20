interface Loadable {
  loading: boolean;
  success: boolean;
  error: any;
}

const createDefaultLoadable = (): Loadable => ({
  loading: false,
  success: false,
  error: null,
});

const onLoadableLoad = <T extends Loadable>(loadable: T): T =>
  ({
    ...(loadable as any),
    loading: true,
    success: false,
    error: null,
  } as T);

const onLoadableSuccess = <T extends Loadable>(loadable: T): T =>
  ({
    ...(loadable as any),
    loading: false,
    success: true,
    error: null,
  } as T);

const onLoadableError = <T extends Loadable>(loadable: T, error: any): T =>
  ({
    ...(loadable as any),
    loading: false,
    success: false,
    error: error,
  } as T);

export {
  Loadable,
  createDefaultLoadable,
  onLoadableLoad,
  onLoadableSuccess,
  onLoadableError,
};
