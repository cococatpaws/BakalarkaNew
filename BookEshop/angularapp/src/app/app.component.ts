import { HttpClient } from '@angular/common/http';
import { Component, HostListener, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { AuthDataService } from './services/auth-data.service';
import { ShoppingBasketService } from './services/shopping-basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'angularapp';

  constructor(private authService: AuthService, private authDataService: AuthDataService, private shoppingBasketService: ShoppingBasketService) { }

  ngOnInit() {
    if (sessionStorage.getItem('token') != null) {
      this.authService.initializeAuthDataAfterReload();
    }

    if (localStorage.length > 0) {
      this.shoppingBasketService.initializeShoppingBasket();
    }
  }
}

