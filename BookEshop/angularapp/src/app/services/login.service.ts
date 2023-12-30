import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Register } from '../interfaces/register.model';
import { Login } from '../interfaces/login.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private apiUrl = 'https://localhost:7073'; // to kde mi bezi backend

  constructor(private http: HttpClient) { }

  login(data: Login): Observable<Login> {
    return this.http.post<Register>(`${this.apiUrl}/login`, data);
  }

  registerUser(data: Register): Observable<Register> {
    return this.http.post<Register>(`${this.apiUrl}/register`, data);
  }


}
