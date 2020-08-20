// The concept of "loadables" was inspired by https://medium.com/angular-in-depth/handle-api-call-state-nicely-445ab37cc9f8

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

const onLoadableLoad = (loadable: Loadable): Loadable => ({
  ...loadable,
  loading: true,
  success: false,
  error: null,
});

const onLoadableSuccess = (loadable: Loadable): Loadable => ({
  ...loadable,
  loading: false,
  success: true,
  error: null,
});

const onLoadableError = (loadable: Loadable, error: any): Loadable => ({
  ...loadable,
  loading: false,
  success: false,
  error: error,
});

export {
  Loadable,
  createDefaultLoadable,
  onLoadableLoad,
  onLoadableSuccess,
  onLoadableError,
};
export default Loadable;
