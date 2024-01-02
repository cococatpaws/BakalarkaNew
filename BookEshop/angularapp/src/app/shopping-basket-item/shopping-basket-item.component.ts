import { Component, Input } from '@angular/core';
import { ShoppingBasketItem } from '../interfaces/shopping-basket-item';

@Component({
  selector: 'app-shopping-basket-item',
  templateUrl: './shopping-basket-item.component.html',
  styleUrls: ['./shopping-basket-item.component.css']
})
export class ShoppingBasketItemComponent {
  @Input() item!: ShoppingBasketItem;


}
