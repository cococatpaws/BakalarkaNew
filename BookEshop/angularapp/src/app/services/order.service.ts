import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject, map, tap } from 'rxjs';
import { Order } from '../interfaces/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = 'https://localhost:7073'; // to kde mi bezi backend
  constructor(private http: HttpClient) { }

  placeOrder(order: Order): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/order`, order);
  }
}
