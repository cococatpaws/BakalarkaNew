import { Component } from '@angular/core';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent {
  bookDisplays: string[] = ["Nové", "Populárne", "Zľavy"];
  bookDisplaySelected: string = "Nové";

  onClick(event: Event, num: number) {
    event.preventDefault();
    this.bookDisplaySelected = this.bookDisplays[num];
  }
}
