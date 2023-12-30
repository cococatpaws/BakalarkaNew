import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Login } from '../interfaces/login.model'
import { LoginService } from '../services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private dialogRef: MatDialogRef<LoginComponent>, private router: Router, private loginService: LoginService) { }

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
      next: (response: Login) => {
        console.log('Údaje úspešne odoslané na server', response);
        this.show = true;
        setTimeout(() => {
          this.router.navigate(['home']);
          this.closeDialog();
        }, 1000);
        //Sem pridat presmerovanie na hlavnu stranku, kde uz bude zobrazeny aj profil atd
        
      },
      error: (error: any) => {
        console.error('Chyba pri odosielaní údajov na server', error);
        this.showInvalid = true;
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
