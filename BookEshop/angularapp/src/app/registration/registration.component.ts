import { Component } from '@angular/core';
import { Register } from '../interfaces/register.model';
import { LoginService } from '../services/login.service';
import { NotificationService } from '../services/notification.service';

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

  constructor(private loginService: LoginService, private notificationService: NotificationService) { }
    

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
