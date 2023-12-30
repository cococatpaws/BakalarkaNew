import { Component } from '@angular/core';
import { Register } from '../interfaces/register.model';
import { LoginService } from '../services/login.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {
    usernameR: string = "";
  emailR: string = "";
  passwordR: string = "";
  passwordConfirmR: string = "";
  nameR: string = "";
  surnameR: string = "";
  phoneNumberR: string = "";
  streetR: string = "";
  addressNumberR: string = "";
  cityR: string = "";
  postCodeR: string = "";
  countryR: string = "";

  constructor(private loginService: LoginService) { }
    

    onSubmit() {
      const user: Register = {
        username: this.usernameR,
        email: this.emailR,
        password: this.passwordR,
        name: this.nameR,
        surname: this.surnameR,
        phoneNumber: this.phoneNumberR,
        street: this.streetR,
        addressNumber: this.addressNumberR,
        city: this.cityR,
        postCode: this.postCodeR,
        country: this.countryR
      };

      this.loginService.registerUser(user).subscribe({
        next: (response: Register) => {
          console.log('Údaje úspešne odoslané na server', response);
          //Sem pridat popup okno co informuje o uspesnej registracii
          window.location.reload();
        },
        error: (error: any) => {
          console.error('Chyba pri odosielaní údajov na server', error);
        }
      });
    }
}
