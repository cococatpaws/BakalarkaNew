import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { LoginComponent } from '../login/login.component';
import { AuthDataService } from '../services/auth-data.service';
import { AuthService } from '../services/auth.service';
import { ShoppingBasketService } from '../services/shopping-basket.service';


@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrls: ['./navigation-bar.component.css']
})
export class NavigationBarComponent {
  filter: string = "Názov";
  drodpownOptions: string[] = ["Názov", "Autor"];
  role: string = "";
  numberOfItems: number = 0;

  constructor(private router: Router, private dialog: MatDialog, private authDataService: AuthDataService, private authService: AuthService,
                private shoppingBasketService: ShoppingBasketService) { }

  ngOnInit() {
    this.authDataService.getRole().subscribe(response =>
    {
      this.role = response
    })

    this.shoppingBasketService.getTotalNumberOfItems().subscribe(response => {
      this.numberOfItems = response;
    });
  }

  menuItems: { text: string; link: string; disabled?: boolean }[] = [
    { text: 'Domov', link: '/home', disabled: false },
    { text: 'Domov', link: '/home', disabled: false },
    { text: 'Domov', link: '/home', disabled: false },
    { text: 'Domov', link: '/home', disabled: false },
  ];

  selectItem(parFilter: string) {
    this.filter = parFilter;
  }

  openLoginPopup() {
    this.dialog.open(LoginComponent, {
      disableClose: true, 
    });
  }

  logout() {
    this.authService.logout();
    //pridat snackbar message - bol si uspesne odhlaseny
  }
}
