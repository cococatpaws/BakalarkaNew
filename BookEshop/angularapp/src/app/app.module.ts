import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { SidebarShoppingInfoComponent } from './sidebar-shopping-info/sidebar-shopping-info.component';
import { NavigationBarComponent } from './navigation-bar/navigation-bar.component';
import { HomePageComponent } from './home-page/home-page.component';
import { FooterComponent } from './footer/footer.component';
import { AboutUsComponent } from './footer-pages/about-us/about-us.component';
import { InStoreReservationComponent } from './footer-pages/in-store-reservation/in-store-reservation.component';
import { BooksPageComponent } from './books-page/books-page.component';
import { AppRoutingModule } from './app-routing.module';
import { AddPageComponent } from './add-page/add-page.component';

@NgModule({
  declarations: [
    AppComponent,
    SidebarShoppingInfoComponent,
    NavigationBarComponent,
    HomePageComponent,
    FooterComponent,
    AboutUsComponent,
    InStoreReservationComponent,
    BooksPageComponent,
    AddPageComponent
  ],
  imports: [
    BrowserModule, HttpClientModule, AppRoutingModule, FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
