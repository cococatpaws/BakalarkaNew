import { Component } from '@angular/core';

@Component({
  selector: 'app-books-page',
  templateUrl: './books-page.component.html',
  styleUrls: ['./books-page.component.css']
})
export class BooksPageComponent {
  barFilters: string[] = ['Predvolené', 'Najlacnejšie', 'Najdrahšie', 'Bestsellery', 'Novinky', 'Hodnotenie', 'Zľavy'];
  languages: string[] = ['Slovenčina', 'Čeština', 'Angličtina', 'Nemčina', 'Ruština'];
  genres: string[] = ['Romantické', 'Fantasy', 'Thriller', 'Náučné', 'Pre deti', 'Detektívky', 'Poézia', 'Klasika'];
  newReleases: string[] = ['Tento mesiac', 'Tento rok']

  sidebarShown: boolean = false;

  showSidebar() {
    this.sidebarShown = !this.sidebarShown;
    console.log(this.sidebarShown);
  }
}
