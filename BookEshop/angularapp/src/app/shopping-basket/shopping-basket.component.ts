import { Component } from '@angular/core';
import { ShoppingBasketItem } from '../interfaces/shopping-basket-item';
import { ShoppingBasketService } from '../services/shopping-basket.service';
import { Router } from '@angular/router';
import { BookPageService } from '../services/book-page.service';
import { ShoppingBasketItemComponent } from '../shopping-basket-item/shopping-basket-item.component';

@Component({
  selector: 'app-shopping-basket',
  templateUrl: './shopping-basket.component.html',
  styleUrls: ['./shopping-basket.component.css']
})
export class ShoppingBasketComponent {
  shoppingBasketItems: any[] = [];
  numberOfItems: number = 0;
  totalNumberOfItems: number = 0;
  totalPrice: number = 0;

  constructor(private shoppingBasketService: ShoppingBasketService, private router: Router, private bookService: BookPageService) { }

  ngOnInit() {
    this.update();
  }

  update() {
    this.updateItems();
    this.updateNumberOfItems();
    this.bookDeletion()
    this.updateTotalprice();
  }

  bookDeletion() {
    this.bookService.getBookDeleted().subscribe(response => {
      this.shoppingBasketService.removeItem(response.toString());
    });
  }

  updateItems() {
    this.shoppingBasketService.getShoppingBasketItems().subscribe(items => {
      this.shoppingBasketItems = items;
      if (this.shoppingBasketItems != undefined) {
        this.numberOfItems = this.shoppingBasketItems.length;
      }
    });
  }

  updateNumberOfItems() {
    this.shoppingBasketService.getTotalNumberOfItems().subscribe(response => {
      this.totalNumberOfItems = response;
    });
  }

  updateTotalprice() {
    this.shoppingBasketService.getTotalPrice().subscribe(response => {
      this.totalPrice = response;
    })
  }

  removeFromBasket(bookId: number) {
    this.shoppingBasketService.removeItem(bookId.toString());
  }

  lowerAmountOfItem(bookId: ShoppingBasketItem) {
    this.shoppingBasketService.lowerAmountOfItem(bookId);
  }

  increaseAmountOfItem(bookId: ShoppingBasketItem) {
    this.shoppingBasketService.increaseAmountOfItem(bookId);
  }

  order() {
    const shoppingBasketItemsJson = JSON.stringify(this.shoppingBasketItems);
    const totalPriceParam = this.totalPrice;
    this.router.navigate(['/objednavka', { shoppingBasketItems: shoppingBasketItemsJson, totalPrice: totalPriceParam }]);
  }

}
