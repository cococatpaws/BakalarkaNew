import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Login } from '../interfaces/login.model'
import { AuthService } from '../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NotificationService } from '../services/notification.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  show: boolean = false;
  showInvalid: boolean = false;
  loginForm!: FormGroup;

  constructor(private dialogRef: MatDialogRef<LoginComponent>, private router: Router, private loginService: AuthService, private formBuilder: FormBuilder,
              private notificationService :NotificationService) { }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  closeDialog() {
    this.dialogRef.close();
  }

  

  submit() {
    if (this.loginForm.valid) {
      const user: Login = {
        username: this.loginForm.get('username')?.value,
        password: this.loginForm.get('password')?.value,
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
        },
        error: (error: any) => {
          console.error('Chyba pri odosielaní údajov na server', error);
          this.showInvalid = true;
          this.show = false;
        }
      });
    }
  }



  redirect() {
    this.closeDialog();
  }
}
