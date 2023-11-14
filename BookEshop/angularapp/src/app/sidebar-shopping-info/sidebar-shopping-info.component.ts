import { Component } from '@angular/core';

@Component({
  selector: 'app-sidebar-shopping-info',
  templateUrl: './sidebar-shopping-info.component.html',
  styleUrls: ['./sidebar-shopping-info.component.css']
})
export class SidebarShoppingInfoComponent {
  links = [
    { title: "Doprava", link: "" },
    { title: "Platba", link: "" },
    { title: "Reklamácie", link: "" },
    { title: "Rezervácia v predajni", link: "/rezervacia-v-knihkupectve" },
  ]
}
