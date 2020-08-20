import { createDefaultLoadable, Loadable } from "../../loadable/loadable";
import { UsersActionsUnion, UsersActionsTypes } from "./users.actions";
import { withLoadable } from "../../loadable/with-loadable";
import { User } from "../../app/models";

interface UserList extends Loadable {
  users?: User[];
}

const createDefaultUsers = (): UserList => ({
  ...createDefaultLoadable(),
  users: [],
});

const baseUsersReducer = (
  state: UserList = createDefaultUsers(),
  action: UsersActionsUnion
): UserList => {
  console.log(action);
  switch (action.type) {
    case UsersActionsTypes.LoadSuccess:
      return {
        ...state,
        users: action.payload.users,
      };
    case UsersActionsTypes.LoadUserSuccess:
      return {
        ...state,
        users: state.users
          .filter((x) => x.id !== action.payload.user.id)
          .concat(action.payload.user),
      };
    case UsersActionsTypes.UpdateSuccess:
      console.log("UPDATE REDUCER");
      return {
        ...state,
        users: state.users
          .filter((x) => x.id !== action.payload.user.id)
          .concat(action.payload.user),
      };
    default:
      return state;
  }
};

const UsersReducer = (state: UserList, action: UsersActionsUnion): UserList =>
  withLoadable(baseUsersReducer, {
    loadingActionType: [
      UsersActionsTypes.Load,
      UsersActionsTypes.LoadUser,
      UsersActionsTypes.Update,
    ],
    successActionType: [
      UsersActionsTypes.LoadSuccess,
      UsersActionsTypes.LoadUserSuccess,
      UsersActionsTypes.UpdateSuccess,
    ],
    errorActionType: [
      UsersActionsTypes.LoadError,
      UsersActionsTypes.LoadUserError,
      UsersActionsTypes.UpdateError,
    ],
  })(state, action);

export { UsersReducer, UserList, createDefaultUsers };
