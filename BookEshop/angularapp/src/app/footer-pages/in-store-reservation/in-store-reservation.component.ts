import { Component } from '@angular/core';
import { SidebarShoppingInfoComponent } from 'src/app/sidebar-shopping-info/sidebar-shopping-info.component';

@Component({
  selector: 'app-in-store-reservation',
  templateUrl: './in-store-reservation.component.html',
  styleUrls: ['./in-store-reservation.component.css']
})
export class InStoreReservationComponent {
  sidebarShown: boolean = false;

  showSidebar() {
    this.sidebarShown = !this.sidebarShown;
    console.log(this.sidebarShown);
  }
}
