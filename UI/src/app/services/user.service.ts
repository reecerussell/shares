import { Injectable } from "@angular/core";
import * as Constants from "../constants";

@Injectable({
  providedIn: "root",
})
export class UserService {
  listeners: Map<string, any>;

  constructor() {
    this.listeners = new Map<string, any>();
  }

  Listen(key: string, callback: any): void {
    this.listeners.set(key, callback);
  }

  Unlisten(key: string): void {
    this.listeners.delete(key);
  }

  Login(token: string, expires: number): void {
    // Convert expiry unix time from UTC to local time.
    expires *= 1000;
    const expiryDate = new Date();
    expiryDate.setTime(expires);

    localStorage.setItem(Constants.AccessTokenKey, token);
    localStorage.setItem(
      Constants.AccessTokenExpiryKey,
      expiryDate.getTime().toString()
    );

    trigger(this.listeners);
  }

  Logout(): void {
    localStorage.removeItem(Constants.AccessTokenKey);
    localStorage.removeItem(Constants.AccessTokenExpiryKey);

    trigger(this.listeners);
  }

  IsAuthenticated(): boolean {
    const token = localStorage.getItem(Constants.AccessTokenKey);
    if (!token) {
      return false;
    }

    const time = localStorage.getItem(Constants.AccessTokenExpiryKey);
    const expiry = new Date(parseInt(time));

    return !(expiry < new Date());
  }

  GetId(): string {
    return getPayload(localStorage.getItem(Constants.AccessTokenKey))[
      Constants.ClaimTypes.UserId
    ];
  }
}

const trigger = (actions: Map<string, any>) => {
  actions.forEach((callback) => callback());
};

const getPayload = (token: string) => {
  if (!token) {
    return null;
  }

  const parts = token.split(".");
  if (parts.length < 2) {
    return null;
  }

  const payloadData = atob(parts[1]);

  return JSON.parse(payloadData);
};
