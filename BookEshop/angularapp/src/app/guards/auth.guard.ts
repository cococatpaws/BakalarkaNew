import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { AuthDataService } from '../services/auth-data.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authentificationDataService: AuthDataService, private authService: AuthService, private router: Router) { }


  canActivate(route: ActivatedRouteSnapshot): boolean {
    console.log(sessionStorage);
    if (this.authentificationDataService.getIsLoggedIn()) {
      // Check if the user has the required role(s)
      console.log("test");
      console.log("Rola: " + this.authService.getRole());
      var userRole = "";

      this.authentificationDataService.getRole().subscribe(response => {
        userRole = response;
      });

      const requiredRoles = route.data['requiredRoles'] as string[];

      if (!requiredRoles || requiredRoles.includes(userRole)) {
        return true;
      } else {
        // Redirect to unauthorized page or handle it in a way that fits your application
        console.log("Unaithorized");
        this.router.navigate(['/home']);
        return false;
      }
    } else {
      // Redirect to login page if not logged in
      this.router.navigate(['/home']);
      return false;
    }
  }
 }

  


