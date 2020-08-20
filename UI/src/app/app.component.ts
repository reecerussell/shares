import { Component, OnInit } from "@angular/core";
import { Router, NavigationEnd } from "@angular/router";
import { UserService } from "./services/user.service";

@Component({
  // tslint:disable-next-line
  selector: "body",
  templateUrl: "app.component.html",
})
export class AppComponent implements OnInit {
  isLoggedIn: boolean = false;

  constructor(private router: Router, private user: UserService) {
    this.isLoggedIn = user.IsAuthenticated();
  }

  ngOnInit() {
    setInterval(() => {
      if (!this.user.IsAuthenticated() && this.isLoggedIn) {
        this.user.Logout();
      }
    }, 5000);

    this.user.Listen(
      "app",
      () => (this.isLoggedIn = this.user.IsAuthenticated())
    );

    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });
  }

  ngOnDestroy() {
    this.user.Unlisten("app");
  }
}
