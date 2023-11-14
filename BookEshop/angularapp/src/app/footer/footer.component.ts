import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent {
  links = [
    { title: "O nás", link: "" },
    { title: "Doprava", link: "" },
    { title: "Platba", link: "" },
    { title: "Kontakt", link: "" },
    { title: "Reklamácie", link: "" },
    { title: "E-Knihy", link: "" },
    { title: "Audioknihy", link: "" },
    { title: "Rezervácia v kníhkupectve", link: "/rezervacia-v-knihkupectve" },
  ]


}
