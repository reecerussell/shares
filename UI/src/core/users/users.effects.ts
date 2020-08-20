import { Injectable } from "@angular/core";
import { Actions, Effect, ofType } from "@ngrx/effects";
import { switchMap } from "rxjs/operators";
import {
  UsersActionsTypes,
  LoadUsersError,
  LoadUsersSuccess,
  LoadUser,
  LoadUserError,
  LoadUserSuccess,
  Update,
  UpdateSuccess,
  UpdateError,
} from "./users.actions";
import { ApiService } from "../../app/services/api.service";

@Injectable()
export class UsersEffects {
  constructor(private actions$: Actions, private api: ApiService) {}

  @Effect()
  LoadUsers = this.actions$.pipe(
    ofType(UsersActionsTypes.Load),
    switchMap((action) => {
      return this.api.Users.List()
        .then(({ ok, error, data }) => {
          if (ok) {
            return new LoadUsersSuccess({ users: data });
          } else {
            return new LoadUsersError(error);
          }
        })
        .catch((error) => new LoadUsersError(error));
    })
  );

  @Effect()
  LoadUser = this.actions$.pipe(
    ofType(UsersActionsTypes.LoadUser),
    switchMap((action: LoadUser) => {
      return this.api.Users.Get(action.id)
        .then(({ ok, error, data }) => {
          if (ok) {
            return new LoadUserSuccess({ user: data });
          } else {
            return new LoadUserError(error);
          }
        })
        .catch((error) => new LoadUserError(error));
    })
  );

  @Effect()
  Update = this.actions$.pipe(
    ofType<Update>(UsersActionsTypes.Update),
    switchMap(async (action: Update) => {
      return await this.api.Users.Update(action.user)
        .then(({ ok, error }) => {
          if (ok) {
            return new UpdateSuccess({ user: action.user });
          } else {
            return new UpdateError(error);
          }
        })
        .catch((error) => new UpdateError(error));
    })
  );
}
