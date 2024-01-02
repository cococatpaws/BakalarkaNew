import { Component, OnInit } from '@angular/core';
import { Register } from '../interfaces/register.model';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { passwordMatchValidator } from '../helpers/custom-validator';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {
  registrationForm!: FormGroup;

  constructor(private loginService: AuthService, private notificationService: NotificationService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.registrationForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      passwordRepeat: ['', [Validators.required]],
      name: ['', [Validators.required, Validators.maxLength(50)]],
      surname: ['', [Validators.required, Validators.maxLength(50)]],
      phoneNumber: ['', [Validators.required, Validators.maxLength(13)]],
      street: ['', [Validators.required, Validators.maxLength(50)]],
      addressNumber: ['', [Validators.required, Validators.maxLength(10)]],
      city: ['', [Validators.required, Validators.maxLength(50)]],
      postCode: ['', [Validators.required, Validators.maxLength(5)]],
      country: ['', [Validators.required]],
    });
  }

  onSubmit() {
    if (this.registrationForm.valid) {
      const user: Register = {
        username: this.registrationForm.get('username')?.value,
        email: this.registrationForm.get('email')?.value,
        password: this.registrationForm.get('password')?.value,
        name: this.registrationForm.get('name')?.value,
        surname: this.registrationForm.get('surname')?.value,
        phoneNumber: this.registrationForm.get('phoneNumber')?.value,
        street: this.registrationForm.get('street')?.value,
        addressNumber: this.registrationForm.get('addressNumber')?.value,
        city: this.registrationForm.get('city')?.value,
        postCode: this.registrationForm.get('postCode')?.value,
        country: this.registrationForm.get('country')?.value
      };

      this.loginService.registerUser(user).subscribe({
        next: (response: Register) => {
          this.notificationService.displayMessage("Registrácia bola úspešná! Môžeš sa prihlásiť", "success");
          //Sem pridat popup okno co informuje o uspesnej registracii
          window.location.reload();
        },
        error: (error: any) => {
          this.notificationService.displayMessage("Registrácia sa nepodarila!", "warning");
        }
      });
    }
    }
}
