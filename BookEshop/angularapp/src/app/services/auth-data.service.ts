import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthDataService {
  private usernameSubject$: BehaviorSubject<string> = new BehaviorSubject<string>("");
  private roleSubject$: BehaviorSubject<string> = new BehaviorSubject<string>("");
  private isLoggedInSubject$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor() { }

  setData(username: string, role: string, isLoggedIn: boolean) {
    this.usernameSubject$.next(username);
    this.roleSubject$.next(role);
    this.isLoggedInSubject$.next(isLoggedIn);
  }

  public getRole() {
    return this.roleSubject$.asObservable();
  }

  public getUsername() {
    return this.usernameSubject$.asObservable();
  }

  public getIsLoggedIn() {
    return this.isLoggedInSubject$.asObservable();
  }
}
