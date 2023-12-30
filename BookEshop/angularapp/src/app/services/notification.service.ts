import { Injectable } from '@angular/core';
import { SnackbarComponent } from '../snackbar/snackbar.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  constructor(private snackBar: MatSnackBar) { }

  displayMessage(message: string, messageType: "info" | "success" | "warning" | "error"): void {
    this.snackBar.openFromComponent(
      SnackbarComponent,
      {
        duration: 5000,
        data: {
          message: message,
          messageType: messageType,
        },
        horizontalPosition: 'center',
        verticalPosition: 'bottom',
        panelClass: 'custom-snackbar',
      }
    );
  }
}
