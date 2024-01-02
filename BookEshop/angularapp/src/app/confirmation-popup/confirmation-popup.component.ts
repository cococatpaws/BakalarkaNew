import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BookPageService } from '../services/book-page.service';
import { NotificationService } from '../services/notification.service';

@Component({
  selector: 'app-confirmation-popup',
  templateUrl: './confirmation-popup.component.html',
  styleUrls: ['./confirmation-popup.component.css']
})
export class ConfirmationPopupComponent {

  constructor(private dialogRef: MatDialogRef<ConfirmationPopupComponent>, @Inject(MAT_DIALOG_DATA) public data: any, private bookService: BookPageService,
              private notificationService: NotificationService) { }

  ngOnInit() {
  }

  closeDialog() {
    this.dialogRef.close();
  }

  delete() {
    if (this.data.bookId != undefined && this.data.bookId > 0) {
      this.bookService.deleteBook(this.data.bookId).subscribe({
        next: () => {
          this.closeDialog();
          this.bookService.setBookDeleted(this.data.bookId);
          window.location.reload();

          this.notificationService.displayMessage("Kniha " + this.data.bookId + " bola úspešne odstránená!", "success");
        },
        error: (error) => {
          this.notificationService.displayMessage("Nastala chyba pri odstraňovaní knihy!", "warning");
        },
      });
    }
  }
}
