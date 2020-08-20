import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";

import { UsersComponent } from "./users.component";
import { EditComponent } from "./edit/edit.component";

const routes: Routes = [
  {
    path: "",
    data: {
      title: "Users",
    },
    children: [
      {
        path: "",
        component: UsersComponent,
      },
      {
        path: ":id",
        data: {
          title: "Edit",
        },
        component: EditComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UsersRoutingModule {}
