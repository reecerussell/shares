import { UserList, createDefaultUserList } from "../models/user";
import { UsersActions, UsersActionsTypes } from "../actions/users";
import {
  onLoadableLoad,
  onLoadableSuccess,
  onLoadableError,
} from "../models/loadable";

const UsersReducer = (
  state: UserList = createDefaultUserList(),
  action: UsersActions
): UserList => {
  switch (action.type) {
    case UsersActionsTypes.Load:
      return onLoadableLoad(state);
    case UsersActionsTypes.LoadSuccess:
      return {
        ...onLoadableSuccess(state),
        users: action.payload.users,
      };
    case UsersActionsTypes.LoadError:
      return onLoadableError(state, action.error);
    default:
      return state;
  }
};

export { UsersReducer };
