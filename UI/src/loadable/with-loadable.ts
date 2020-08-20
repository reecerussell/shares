import {
  Loadable,
  onLoadableError,
  onLoadableLoad,
  onLoadableSuccess,
} from "./loadable";

interface Action {
  type: string;
}

interface ReducerFunction<T, U extends Action> {
  (state: T, action: U): T;
}

interface ActionTypes {
  loadingActionType: string | string[];
  successActionType: string | string[];
  errorActionType: string | string[];
}

const withLoadable = <T extends Loadable, U extends Action = Action>(
  reducer: ReducerFunction<T, U>,
  { loadingActionType, successActionType, errorActionType }: ActionTypes
) => (state: T, action: U) => {
  if (matchType(loadingActionType, action.type)) {
    state = onLoadableLoad(state);
  }

  if (matchType(successActionType, action.type)) {
    state = onLoadableSuccess(state);
  }

  if (matchType(errorActionType, action.type)) {
    state = onLoadableError(state, (action as any).error);
  }

  return reducer(state, action);
};

const matchType = (actionType: string | string[], type: string): boolean =>
  typeof actionType === "string"
    ? actionType === type
    : actionType.indexOf(type) !== -1;

export { Action, ReducerFunction, ActionTypes, withLoadable };
