import { Component } from "@angular/core";
import { ApiService } from "../../services/api.service";
import { UserService } from "../../services/user.service";

@Component({
  selector: "app-login",
  templateUrl: "login.component.html",
})
export class LoginComponent {
  email: string = "";
  password: string = "";

  constructor(private api: ApiService, private user: UserService) {}

  async onSubmit() {
    const { ok, data, error } = await this.api.Login(this.email, this.password);
    if (!ok) {
      console.error(error);
      return;
    }

    const { token, expires } = data;
    this.user.Login(token, expires);
  }
}
