import { HttpClient } from '@angular/common/http';
import { Component, HostListener, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { AuthDataService } from './services/auth-data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'angularapp';

  constructor(private authService: AuthService, private authDataService: AuthDataService) { }

  ngOnInit() {
    //this.authService.initializeAuthState();
    /*var role = "";
    var username = ""; 

    this.authDataService.getRole().subscribe(response => {
      role = response;
    });

    this.authDataService.getUsername().subscribe(response => {
      username = response;
    })
    console.log("Rola v app comp:" + role);
    console.log("Username v app comp: " + username);
    console.log("Local storage app comp: " + localStorage.getItem('token'));

    if (localStorage.getItem('token') != null) {
      this.authService.initializeAuthDataAfterReload();
    }*/
    if (sessionStorage.getItem('token') != null) {
      this.authService.initializeAuthDataAfterReload();
    }
  }
 

  /*@HostListener("window:unload", ["$event"])
  clearLocalStorage(event) {
    localStorage.clear();
  }*/
}

