import { Component, ElementRef, ViewChild } from '@angular/core';
import { BookFormat } from '../interfaces/Enums/book-format.enum';
import { BookLanguage } from '../interfaces/Enums/book-language.enum';
import { Genre } from '../interfaces/Enums/genre.enum';
import { Author } from '../interfaces/author.model';
import { Book } from '../interfaces/book.model';
import { BookPageService } from '../services/book-page.service';
import { NotificationService } from '../services/notification.service';


@Component({
  selector: 'app-add-page',
  templateUrl: './add-page.component.html',
  styleUrls: ['./add-page.component.css']
})
export class AddPageComponent {
  bookFormat = BookFormat;
  bookLanguage = BookLanguage;
  bookGenre = Genre;

  title: string = '';
  description: string = '';
  quantityInStock: number = 0;
  coverImageUrl: string = ''
  genre!: Genre;
  price: number = 0;
  publisher: string = '';
  numberOfPages: number = 0;
  format!: BookFormat;
  publicationDate!: Date;
  language!: BookLanguage;
  selectedAuthors: string = '';

  bookAuthors: Author[] = [];

  bookImage: File | undefined = undefined;

  bookId: number = 0;

  @ViewChild('fileInput') fileInput: ElementRef<HTMLInputElement> | undefined;
  isChecked: boolean = false;

  constructor(private bookPageService: BookPageService, private notificationService: NotificationService) { }

  onSubmit(): void {
    this.bookAuthors = this.mapBookAuthors(this.selectedAuthors);
    console.log(this.genre);

    const book: Book = {
      title: this.title,
      description: this.description,
      quantityInStock: this.quantityInStock,
      coverImageURL: this.coverImageUrl,
      genre: this.genre,
      price: this.price,
      publisher: this.publisher,
      numberOfPages: this.numberOfPages,
      bookFormat: this.format,
      publicationDate: this.publicationDate,
      bookLanguage: this.language,
      booksAuthors: this.bookAuthors,
    };

    console.log(book);
    console.log(this.bookImage);

    this.bookPageService.saveBookInDB(book).subscribe({
      next: (response: any) => {
        console.log(response);
        this.notificationService.displayMessage("Kniha bola pridaná do databázy!", "info");

        if (response.bookId != null && response.bookId > 0) {
          this.bookId = response.bookId;

          var formData = new FormData();
          formData.append('bookId', this.bookId.toString());
          formData.append('coverImage', this.bookImage as Blob);

          this.bookPageService.saveBookPictureInDB(formData).subscribe({
            next: (response: any) => {

            },
            error: (error: any) => {

            }
          });
        }
      },
      error: (error: any) => {
        this.notificationService.displayMessage("Knihu sa nepodarilo pridať do databázy!", "warning");
        console.log(error);
      }
    });
  }

  removeImgSelection() {
    if (this.fileInput) {
      this.bookImage = undefined;
      this.fileInput.nativeElement.value = "";
    }
  }


  onFileChange(event: any) {
    this.bookImage = event.target.files[0];
  }

  mapBookAuthors(authorsString: string): Author[] {
    // Rozdeliť reťazec autorov pomocou čiarky
    const authorNames = authorsString.split(',');

    // Vytvoriť pole objektov typu Author
    const authors: Author[] = authorNames.map((authorName, index) => {
      // Rozdeliť meno autora podľa medzier
      const nameParts = authorName.trim().split(' ');

      // Ak je jeden medzera, použite iba meno a priezvisko
      if (nameParts.length === 2) {
        const [name, surname] = nameParts;
        return { name, surname };
      }

      // Ak sú dve medzery, použite meno, stredné meno a priezvisko
      if (nameParts.length === 3) {
        const [name, middleName, surname] = nameParts;
        return { name, middleName, surname };
      }

      // Inak vráť prázdny objekt alebo ošetrenie pre iný prípad
      return { name: '', middleName: '', surname: '' };
    });

    return authors;
  }
}
