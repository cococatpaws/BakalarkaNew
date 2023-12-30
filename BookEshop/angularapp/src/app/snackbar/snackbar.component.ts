import { Component, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-snackbar',
  templateUrl: './snackbar.component.html',
  styleUrls: ['./snackbar.component.css'],
  standalone: true
})
export class SnackbarComponent {
  constructor(@Inject(MAT_SNACK_BAR_DATA) public data: any, public snackBarRef: MatSnackBarRef<SnackbarComponent>) {
  }

  get getIcon() {
    switch (this.data.messageType) {
      case 'info':
        return 'bi bi-info-circle-fill';
      case 'error':
        return 'bi bi-x-circle-fill';
      case 'warning':
        return 'bi bi-exclamation-diamond-fill';
      case 'success':
        return 'bi bi-check-circle-fill'
      default:
        return 'bi bi-exclamation-diamond-fill';
    }
  }

  get getTitle() {
    switch (this.data.messageType) {
      case 'info':
        return 'Info';
      case 'error':
        return 'Niečo sa pokazilo';
      case 'warning':
        return 'Varovanie';
      case 'success':
        return 'Úspech'
      default:
        return 'warning';
    }
  }
}
