import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrls: ['./navigation-bar.component.css']
})
export class NavigationBarComponent {
  filter: string = "Názov";
  drodpownOptions: string[] = ["Názov", "Autor"];

  constructor(private router: Router) { }

  menuItems: { text: string; link: string; disabled?: boolean }[] = [
    { text: 'Domov', link: '/home', disabled: false },
    { text: 'Domov', link: '/home', disabled: false },
    { text: 'Domov', link: '/home', disabled: false },
    { text: 'Domov', link: '/home', disabled: false },
  ];

  selectItem(parFilter: string) {
    this.filter = parFilter;
  }


}
