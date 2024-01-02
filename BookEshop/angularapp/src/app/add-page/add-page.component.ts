import { Component, ElementRef, ViewChild, OnInit } from '@angular/core';
import { BookFormat } from '../interfaces/Enums/book-format.enum';
import { BookLanguage } from '../interfaces/Enums/book-language.enum';
import { Genre } from '../interfaces/Enums/genre.enum';
import { Author } from '../interfaces/author.model';
import { Book } from '../interfaces/book.model';
import { BookPageService } from '../services/book-page.service';
import { NotificationService } from '../services/notification.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'app-add-page',
  templateUrl: './add-page.component.html',
  styleUrls: ['./add-page.component.css']
})
export class AddPageComponent {
  bookForm!: FormGroup;

  constructor(private formBuilder: FormBuilder, private bookPageService: BookPageService, private notificationService: NotificationService) { }

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

  ngOnInit() {
    this.bookForm = this.formBuilder.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      quantityInStock: ['', Validators.required],
      genre: ['', Validators.required],
      price: ['', Validators.required],
      publisher: ['', Validators.required],
      numberOfPages: ['', Validators.required],
      bookFormat: ['', Validators.required],
      publicationDate: ['', Validators.required],
      bookLanguage: ['', Validators.required],
      booksAuthors: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.bookForm.valid) {
      var bookAuthorsid = this.bookForm.get('booksAuthors')?.value;
      console.log(bookAuthorsid);
      var bookAuthors = this.mapBookAuthors(this.bookForm.get('booksAuthors')?.value);

      const book: Book = {
        title: this.bookForm.get('title')?.value,
        description: this.bookForm.get('description')?.value,
        quantityInStock: this.bookForm.get('quantityInStock')?.value,
        genre: this.bookForm.get('genre')?.value,
        price: this.bookForm.get('price')?.value,
        publisher: this.bookForm.get('publisher')?.value,
        numberOfPages: this.bookForm.get('numberOfPages')?.value,
        bookFormat: this.bookForm.get('bookFormat')?.value,
        publicationDate: this.bookForm.get('publicationDate')?.value,
        bookLanguage: this.bookForm.get('bookLanguage')?.value,
        booksAuthors: bookAuthors,
        coverImageURL: ""
      };


      this.bookPageService.saveBookInDB(book).subscribe({
        next: (response: any) => {
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
        }
      });
    }
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
