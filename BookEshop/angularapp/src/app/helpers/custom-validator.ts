import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

//redo this somehow
export function passwordMatchValidator(password: any, repeatPassword: any): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    console.log("Heslo: " + password);
    const passwordValue = control.get(password)?.value;
    console.log(passwordValue);
    const repeatPasswordValue = control.get(repeatPassword)?.value;
    console.log("Heslo znova: " + repeatPassword);
    console.log(repeatPasswordValue);

    if (passwordValue === repeatPasswordValue) {
      return null;
    } else {
      return { passwordMismatch: true };
    }
  };
}
