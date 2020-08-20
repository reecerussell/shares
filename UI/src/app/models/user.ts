import { Loadable, createDefaultLoadable } from "./loadable";

interface User {
  id: string;
  firstname: string;
  lastname: string;
  email: string;
}

interface UserList extends Loadable {
  users?: User[];
}

const createDefaultUserList = (): UserList => ({
  ...createDefaultLoadable(),
  users: [],
});

export { User, UserList, createDefaultUserList };
export default User;
