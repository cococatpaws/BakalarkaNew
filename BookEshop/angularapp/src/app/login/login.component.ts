import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Login } from '../interfaces/login.model'
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private dialogRef: MatDialogRef<LoginComponent>, private router: Router, private loginService: AuthService) { }

  closeDialog() {
    this.dialogRef.close();
  }

  usernameL: string = "";
  passwordL: string = "";
  show: boolean = false;
  showInvalid: boolean = false;

  submit() {
    const user: Login = {
      username: this.usernameL,
      password: this.passwordL
    };

    this.loginService.login(user).subscribe({
      next: (response) => {
        this.showInvalid = false;
        this.show = true;
        this.loginService.storeToken(response.token);
        setTimeout(() => {
          this.router.navigate(['home']);
          this.closeDialog();
        }, 1000);
        //Sem pridat presmerovanie na hlavnu stranku, kde uz bude zobrazeny aj profil atd
        
      },
      error: (error: any) => {
        console.error('Chyba pri odosielaní údajov na server', error);
        this.showInvalid = true;
        this.show = false;
      }
    });

    this.clear();
  }

  clear() {
    this.usernameL = "";
    this.passwordL = "";
  }

  redirect() {
    this.closeDialog();
  }
}
