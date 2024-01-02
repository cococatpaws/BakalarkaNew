import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Register } from '../interfaces/register.model';
import { Login } from '../interfaces/login.model';
import { Observable, Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthDataService } from './auth-data.service';
import { Router } from '@angular/router';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7073'; // to kde mi bezi backend
  private jwtHelper = new JwtHelperService();
  private decodedToken: any = "";

  constructor(private http: HttpClient, private authDataService: AuthDataService, private router: Router, private notificationService: NotificationService) {
  }

  login(data: any): Observable<any> {
    return this.http.post<Login>(`${this.apiUrl}/login`, data);
  }

  registerUser(data: Register): Observable<Register> {
    return this.http.post<Register>(`${this.apiUrl}/register`, data);
  }

  logout() {
    sessionStorage.clear();
    this.authDataService.setData("", "", false);
    this.router.navigate(["/home"]);
    this.notificationService.displayMessage("Bol si odhlásený!", "info");
  }

  //ulozenie tokenu do lokalnej pamate
  storeToken(parToken: string) {
    sessionStorage.setItem('token', parToken);
    this.decodedToken = this.tokenDecoding();
    this.authDataService.setData(this.getUsername(), this.getRole(), true);
  }

  //ziskanie tokenu zo session storage
  getToken() {
    return sessionStorage.getItem('token');
  }

  tokenDecoding() {
    const tokenToDecode = this.getToken()!
    var decodedToken = this.jwtHelper.decodeToken(tokenToDecode);
    return this.jwtHelper.decodeToken(tokenToDecode);
    
  }

  getUsername(): string {
    return this.decodedToken.unique_name;
  }

  getRole(): string {
    return this.decodedToken.role;
  }

  initializeAuthDataAfterReload() {
    this.decodedToken = this.tokenDecoding();
    this.authDataService.setData(this.getUsername(), this.getRole(), true)
  }
}
