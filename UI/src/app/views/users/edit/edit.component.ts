import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import { UserList } from "../../../../core/users/users";
import { LoadUser, Update } from "../../../../core/users/users.actions";
import { User } from "../../../models";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-edit-user",
  templateUrl: "./edit.component.html",
})
export class EditComponent implements OnInit {
  users$: Observable<UserList>;
  user: User;

  constructor(
    private store: Store<{ users: UserList }>,
    private route: ActivatedRoute
  ) {
    this.users$ = this.store.select((state) => state.users);
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const id = params.get("id");
      this.store.dispatch(new LoadUser(id));
      this.users$.subscribe((l) => {
        const user = l.users.find((x) => x.id === id);
        this.user = { ...user };
        console.log(this.user);
      });
    });
  }

  onSave(): void {
    const action = new Update(this.user);
    this.store.dispatch(action);
  }
}
