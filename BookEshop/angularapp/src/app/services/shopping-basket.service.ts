import { Injectable } from '@angular/core';
import { ShoppingBasketItem } from '../interfaces/shopping-basket-item';
import { BehaviorSubject } from 'rxjs';
import { BookPageService } from './book-page.service';

@Injectable({
  providedIn: 'root'
})
export class ShoppingBasketService {
  private shoppingBasketItemsSubject: BehaviorSubject<ShoppingBasketItem[]> = new BehaviorSubject<ShoppingBasketItem[]>([]);
  private totalNumberOfItems: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  private totalPrice: BehaviorSubject<number> = new BehaviorSubject<number>(0);

  constructor(private bookService: BookPageService) {
    this.initializeShoppingBasket();
  }

  getShoppingBasketItems() {
    return this.shoppingBasketItemsSubject.asObservable();
  }

  getTotalNumberOfItems() {
    return this.totalNumberOfItems.asObservable(); 
  }

  getTotalPrice() {
    return this.totalPrice.asObservable();
  }

  private setShoppingBasketItems(list: any): void {
    this.shoppingBasketItemsSubject.next(list);
  }

  private setTotalNumberOfItems(numberOfItems: number): void {
    this.totalNumberOfItems.next(numberOfItems);
  }

  private setTotalPrice(price: number): void {
    this.totalPrice.next(price);
  }

  initializeShoppingBasket() {
    this.prepareShoppingBasketItemList();
  }

  saveBookWithExpiration(key: string, shoppingBasketItem: ShoppingBasketItem, expirationHours: number): void {
    var itemInStorage = localStorage.getItem(key);
    if (itemInStorage != null) {
      this.changeAmountOfItem(shoppingBasketItem, "increase");
    } else {
      const expiration = expirationHours * 60 * 60 * 1000;
      const expirationTime = new Date().getTime() + expiration;
      const item = { shoppingBasketItem, expirationTime };
      localStorage.setItem(key, JSON.stringify(item));
    }

    this.prepareShoppingBasketItemList();
  }

  removeItem(key: string): void {
    localStorage.removeItem(key);
    this.prepareShoppingBasketItemList();
  }

  clearBasket(): void {
    const keys = Object.keys(localStorage);

    for (const key of keys) {
      if (this.isNumeric(key)) {
        localStorage.removeItem(key);
      }
    }

    this.prepareShoppingBasketItemList();
  }

  lowerAmountOfItem(item: ShoppingBasketItem) {
    this.changeAmountOfItem(item, "decrease");
    this.prepareShoppingBasketItemList();
  }

  increaseAmountOfItem(item: ShoppingBasketItem) {
    this.changeAmountOfItem(item, "increase");
    this.prepareShoppingBasketItemList();
  }

  changeAmountOfItem(item: ShoppingBasketItem, action: string) {
    var itemInStorage = localStorage.getItem(item.bookId.toString());
    if (itemInStorage != null) {
      var existingItem = JSON.parse(itemInStorage);
      if (action == "increase") {
        existingItem.shoppingBasketItem.bookQuantity++;
        existingItem.shoppingBasketItem.totalPrice = +(
          existingItem.shoppingBasketItem.totalPrice + item.price
        ).toFixed(2);
      } else if (action == "decrease") {
        existingItem.shoppingBasketItem.bookQuantity--;
        existingItem.shoppingBasketItem.totalPrice = +(
          existingItem.shoppingBasketItem.totalPrice - item.price
        ).toFixed(2);
      }
      
      localStorage.setItem(item.bookId.toString(), JSON.stringify(existingItem));
    }
  }


  prepareShoppingBasketItemList(): any {
    const keys = Object.keys(localStorage);
    const shoppingBasketItemList = [];
    var numberOfItems = 0;
    var totalPrice = 0;

    for (const key of keys) {
      if (this.isNumeric(key)) {
        const shoppingBasketItemString = localStorage.getItem(key);

        if (shoppingBasketItemString !== null) {
          const shoppingBasketItemObject = JSON.parse(shoppingBasketItemString);

          const expirationTime = shoppingBasketItemObject.expirationTime;
          const currentTime = new Date().getTime();
          

          if (currentTime > expirationTime) {
            localStorage.removeItem(key);
          } else {
            shoppingBasketItemList.push(shoppingBasketItemObject.shoppingBasketItem);
            numberOfItems += shoppingBasketItemObject.shoppingBasketItem.bookQuantity;
            totalPrice += shoppingBasketItemObject.shoppingBasketItem.totalPrice;
          }
        }
      }
    }

    this.setShoppingBasketItems(shoppingBasketItemList);
    this.setTotalNumberOfItems(numberOfItems);
    this.setTotalPrice(totalPrice);
  }

  isNumeric(key: string): boolean {
    const numericRegex = /^[0-9]+$/;

    return numericRegex.test(key);
  }

  

  
}
