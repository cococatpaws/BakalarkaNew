import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  constructor() { }



  removeItem(key: string): void {
    localStorage.removeItem(key);
  }
}
