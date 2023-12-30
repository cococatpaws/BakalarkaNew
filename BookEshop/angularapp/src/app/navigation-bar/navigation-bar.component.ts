import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { LoginComponent } from '../login/login.component';

@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrls: ['./navigation-bar.component.css']
})
export class NavigationBarComponent {
  filter: string = "Názov";
  drodpownOptions: string[] = ["Názov", "Autor"];

  constructor(private router: Router, private dialog: MatDialog) { }

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
}
