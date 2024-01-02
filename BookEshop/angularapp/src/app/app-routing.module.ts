import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './home-page/home-page.component';
import { InStoreReservationComponent } from './footer-pages/in-store-reservation/in-store-reservation.component';
import { BooksPageComponent } from './books-page/books-page.component';
import { AddPageComponent } from './add-page/add-page.component';
import { EditBookPageComponent } from './edit-book-page/edit-book-page.component';
import { RegistrationComponent } from './registration/registration.component';
import { AuthGuard } from './guards/auth.guard';
import { BookPageComponent } from './book-page/book-page.component';
import { ShoppingBasketComponent } from './shopping-basket/shopping-basket.component';
import { OrderPageComponent } from './order/order-page/order-page.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomePageComponent },
  { path: 'rezervacia-v-knihkupectve', component: InStoreReservationComponent },
  { path: 'knihy', component: BooksPageComponent },
  { path: 'pridaj', component: AddPageComponent, canActivate: [AuthGuard], data: { requiredRoles: ['Admin'] } },
  { path: 'uprav-knihu/:id', component: EditBookPageComponent, canActivate: [AuthGuard], data: { requiredRoles: ['Admin'] } },
  { path: 'registracia', component: RegistrationComponent },
  { path: 'zobraz-knihu/:id', component: BookPageComponent },
  { path: 'nakupny-kosik', component: ShoppingBasketComponent },
  { path: 'objednavka', component: OrderPageComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
