import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import { UserList } from "../../../core/users/users";
import { LoadUsers } from "../../../core/users/users.actions";

@Component({
  selector: "app-users",
  templateUrl: "./users.component.html",
})
export class UsersComponent implements OnInit {
  users$: Observable<UserList>;

  constructor(private store: Store<{ users: UserList }>) {
    this.users$ = this.store.select((state) => state.users);

    this.users$.subscribe(console.log);
  }

  ngOnInit(): void {
    const action = new LoadUsers();
    this.store.dispatch(action);
  }
}
