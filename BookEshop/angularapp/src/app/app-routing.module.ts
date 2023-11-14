import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './home-page/home-page.component';
import { InStoreReservationComponent } from './footer-pages/in-store-reservation/in-store-reservation.component';
import { BooksPageComponent } from './books-page/books-page.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomePageComponent },
  { path: 'rezervacia-v-knihkupectve', component: InStoreReservationComponent },
  { path: 'knihy', component: BooksPageComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
