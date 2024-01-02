import { Component, OnInit } from '@angular/core';
import { ShoppingBasketService } from '../../services/shopping-basket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ShoppingBasketItem } from '../../interfaces/shopping-basket-item';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Order } from '../../interfaces/order';
import { BookInOrder } from '../../interfaces/book-in-order';
import { AuthDataService } from '../../services/auth-data.service';
import { OrderService } from '../../services/order.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-order-page',
  templateUrl: './order-page.component.html',
  styleUrls: ['./order-page.component.css']
})
export class OrderPageComponent {
  itemList: ShoppingBasketItem[] = [];
  totalPrice: number = 0;
  orderForm!: FormGroup;
  username: string = "";
  constructor(private shoppingBasketService: ShoppingBasketService, private route: ActivatedRoute, private formBuilder: FormBuilder, private authDataService: AuthDataService,
  private orderService: OrderService, private notificationService: NotificationService, private router: Router) { }

  ngOnInit() {
    this.getUsername();
    this.shoppingBasketService.getShoppingBasketItems().subscribe(response => {
      this.itemList = response;
    })

    this.shoppingBasketService.getTotalPrice().subscribe(response => {
      this.totalPrice = response;
    })

    this.orderForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      surname: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
      phoneNumber: ['', [Validators.required, Validators.maxLength(13)]],
      country: ['', [Validators.required]],
      city: ['', [Validators.required, Validators.maxLength(50)]],
      street: ['', [Validators.required, Validators.maxLength(50)]],
      addressNumber: ['', [Validators.required, Validators.maxLength(10)]],
      postCode: ['', [Validators.required, Validators.maxLength(5)]],
      paymentTypeId: ['', [Validators.required]],
      shippingTypeId: ['', [Validators.required]],
      orderType: ['', [Validators.required]],
      orderDetails: ['', [Validators.required, Validators.maxLength(200)]],
    });

    console.log(this.itemList);
  }

  onSubmit() {
    var bookInOrder: BookInOrder[] = this.itemList.map(item => {
      return {
        bookId: item.bookId,
        quantityOrdered: item.bookQuantity,
        bookPrice: item.price
      };
    });

    console.log(bookInOrder);

    var order: Order = {
      username: this.username,
      name: this.orderForm.get('name')?.value,
      surname: this.orderForm.get('surname')?.value,
      email: this.orderForm.get('email')?.value,
      phoneNumber: this.orderForm.get('phoneNumber')?.value,
      street: this.orderForm.get('street')?.value,
      addressNumber: this.orderForm.get('addressNumber')?.value,
      city: this.orderForm.get('city')?.value,
      postCode: this.orderForm.get('postCode')?.value,
      country: this.orderForm.get('country')?.value,
      booksInOrder: bookInOrder,
      paymentTypeId: this.orderForm.get('paymentTypeId')?.value,
      shippingTypeId: this.orderForm.get('shippingTypeId')?.value,
      orderType: this.orderForm.get('orderType')?.value,
      orderDetails: this.orderForm.get('orderDetails')?.value,
    };

    this.orderService.placeOrder(order).subscribe({
      next: (response) => {
        this.notificationService.displayMessage("Objednávka bola úspešná!", "success");
        setTimeout(() => {
          this.router.navigate(["/home"]);
        },5000)
        
      },
      error: (error: any) => {
        this.notificationService.displayMessage("Objednávka sa nepodarila!", "warning");
      }
    });

    this.shoppingBasketService.clearBasket();
  }

  getUsername() {
    this.authDataService.getUsername().subscribe(response => {
      this.username = response;
    });
  }


}
