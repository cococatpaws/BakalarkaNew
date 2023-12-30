import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

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
import { BookFormComponent } from './book-form/book-form.component';
import { EditBookPageComponent } from './edit-book-page/edit-book-page.component';
import { LoginComponent } from './login/login.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RegistrationComponent } from './registration/registration.component';
import { SnackbarComponent } from './snackbar/snackbar.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';

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
    AddPageComponent,
    BookFormComponent,
    EditBookPageComponent,
    LoginComponent,
    RegistrationComponent
  ],
  imports: [
    BrowserModule, HttpClientModule, AppRoutingModule, FormsModule, BrowserAnimationsModule, MatDialogModule, MatFormFieldModule, MatInputModule,
    MatButtonModule, MatSnackBarModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
