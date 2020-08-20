import { Injectable } from "@angular/core";
import * as Constants from "../constants";
import { User } from "../models";

interface ApiResponse {
  ok: boolean;
  error: string;
  data: any;
}

const getAuthHeader = () =>
  "Bearer " + localStorage.getItem(Constants.AccessTokenKey);

class UsersService {
  baseUrl: string;

  constructor(baseUrl: string) {
    this.baseUrl = baseUrl + "users";
  }

  async List(): Promise<ApiResponse> {
    const res = await fetch(this.baseUrl, {
      method: "GET",
      headers: {
        Authorization: getAuthHeader(),
      },
    });

    return await parseResponse(res);
  }

  async Get(id: string): Promise<ApiResponse> {
    const res = await fetch(this.baseUrl + `/${id}`, {
      method: "GET",
      headers: {
        Authorization: getAuthHeader(),
      },
    });

    return await parseResponse(res);
  }

  async Update(data: User): Promise<ApiResponse> {
    const res = await fetch(this.baseUrl, {
      method: "PUT",
      headers: {
        Authorization: getAuthHeader(),
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });

    return await parseResponse(res);
  }
}

const parseResponse = async (res: Response): Promise<ApiResponse> => {
  if (res.status === 200) {
    const { data } = await res.json();
    return {
      ok: true,
      error: null,
      data: data,
    };
  }

  const { error } = await res.json();
  return {
    ok: false,
    error: error,
    data: null,
  };
};

@Injectable({
  providedIn: "root",
})
export class ApiService {
  baseUrl: string;

  constructor() {
    this.baseUrl = Constants.APIBase;

    this.Users = new UsersService(this.baseUrl);
  }

  public Users: UsersService;

  async Login(email: string, password: string): Promise<ApiResponse> {
    const res = await fetch(this.baseUrl + "token", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ email, password }),
    });

    return parseResponse(res);
  }
}
