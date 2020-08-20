import { Action } from "@ngrx/store";
import { User } from "../models";

enum UsersActionsTypes {
  Load = "[NEWS PAGE] LOAD NEWS",
  LoadSuccess = "[NEWS PAGE] LOAD NEWS SUCCESS",
  LoadError = "[NEWS PAGE] LOAD NEWS ERROR",
}

class LoadUsers implements Action {
  readonly type = UsersActionsTypes.Load;
}

class LoadUsersSuccess implements Action {
  readonly type = UsersActionsTypes.LoadSuccess;
  constructor(public payload: { users: User[] }) {}
}

class LoadUsersError implements Action {
  readonly type = UsersActionsTypes.LoadError;
  constructor(public error: any) {}
}

type UsersActions = LoadUsers | LoadUsersSuccess | LoadUsersError;

export {
  UsersActionsTypes,
  LoadUsers,
  LoadUsersSuccess,
  LoadUsersError,
  UsersActions,
};
