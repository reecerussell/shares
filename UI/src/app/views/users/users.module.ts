// Angular
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { NgModule } from "@angular/core";

// Components Routing
import { UsersRoutingModule } from "./users-routing.module";
import { UsersComponent } from "./users.component";
import { EditComponent } from "./edit/edit.component";
import { LoadingContainerComponent } from "../../loading/loading.component";

@NgModule({
  imports: [CommonModule, FormsModule, UsersRoutingModule],
  declarations: [UsersComponent, EditComponent, LoadingContainerComponent],
})
export class UsersModule {}
