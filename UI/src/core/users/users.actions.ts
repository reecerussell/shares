import { Action } from "@ngrx/store";
import { User } from "../../app/models";

enum UsersActionsTypes {
  Load = "[USERS] LOAD",
  LoadSuccess = "[USERS] LOAD SUCCESS",
  LoadError = "[USERS] LOAD ERROR",
  LoadUser = "[USER] LOAD",
  LoadUserSuccess = "[USER] LOAD SUCCESS",
  LoadUserError = "[USER] LOAD ERROR",
  Update = "[USER] UPDATE",
  UpdateSuccess = "[USER] UPDATE SUCCESS",
  UpdateError = "[USER] UPDATE ERROR",
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

// User actions
class LoadUser implements Action {
  readonly type = UsersActionsTypes.LoadUser;
  constructor(public id: string) {}
}

class LoadUserSuccess implements Action {
  readonly type = UsersActionsTypes.LoadUserSuccess;
  constructor(public payload: { user: User }) {}
}

class LoadUserError implements Action {
  readonly type = UsersActionsTypes.LoadUserError;
  constructor(public error: any) {}
}

// Update actions
class Update implements Action {
  readonly type = UsersActionsTypes.Update;
  constructor(public user: User) {}
}

class UpdateSuccess implements Action {
  readonly type = UsersActionsTypes.UpdateSuccess;
  constructor(public payload: { user: User }) {}
}

class UpdateError implements Action {
  readonly type = UsersActionsTypes.UpdateError;
  constructor(public error: any) {}
}

type UsersActionsUnion =
  | LoadUsers
  | LoadUsersSuccess
  | LoadUsersError
  | LoadUser
  | LoadUserSuccess
  | LoadUserError
  | Update
  | UpdateSuccess
  | UpdateError;

export {
  UsersActionsTypes,
  UsersActionsUnion,
  LoadUsers,
  LoadUsersError,
  LoadUsersSuccess,
  LoadUser,
  LoadUserSuccess,
  LoadUserError,
  Update,
  UpdateSuccess,
  UpdateError,
};
