 import { Component, ElementRef, ViewChild } from '@angular/core';
import { BookFormat } from '../interfaces/Enums/book-format.enum';
import { BookLanguage } from '../interfaces/Enums/book-language.enum';
import { Genre } from '../interfaces/Enums/genre.enum';
import { Author } from '../interfaces/author.model';
import { Book } from '../interfaces/book.model';
import { BookPageService } from '../services/book-page.service';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from '../services/notification.service';


@Component({
  selector: 'app-edit-book-page',
  templateUrl: './edit-book-page.component.html',
  styleUrls: ['./edit-book-page.component.css']
})
export class EditBookPageComponent {
  book: Book = {
    title: "",
    description: "",
    quantityInStock: 0,
    genre: undefined,
    price: 0,
    publisher: "",
    numberOfPages: 0,
    bookFormat: undefined,
    publicationDate: undefined,
    bookLanguage: undefined,
    booksAuthors: undefined,
    coverImageURL: "",
    deleteImage: false 
};

  authorsString: string = '';
  bookAuthors: Author[] = [];
  bookId: number = 0;
  isChecked: boolean = false;

  bookImage: File | undefined = undefined;
  @ViewChild('fileInput') fileInput: ElementRef<HTMLInputElement> | undefined;

  constructor(private bookPageService: BookPageService, private route: ActivatedRoute, private notificationService: NotificationService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.bookId = +params['id'];
    });

    this.bookPageService.getBookById(this.bookId).subscribe(response => {
      this.mapResponse(response);
    });
  }

  onFileChange(event: any) {
    this.bookImage = event.target.files[0];
    this.book.deleteImage = true;
  }

  removeImgSelection() {
    if (this.fileInput) {
      this.bookImage = undefined;
      this.fileInput.nativeElement.value = "";
    }
  }

  onSubmit(): void {
    if (this.bookImage == undefined) {
      this.book.deleteImage = this.isChecked;
    }

    this.bookAuthors = this.mapBookAuthors(this.authorsString);
    this.book.booksAuthors = this.bookAuthors;

    this.bookPageService.editBookInDB(this.book).subscribe({
      next: (response: any) => {
        this.notificationService.displayMessage("Informácie o knihe bolo úspešne zmenené!", "success");

        //ak this.bookImage != undefined tak sa tu vykona dalsi request na upload obrazku
        if (this.bookImage != undefined) {
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
        this.notificationService.displayMessage("Info o knihe sa nepodarilo zmeniť!", "warning");
      }
    });
  }

  mapBookAuthors(authorsString: string): Author[] {
    const authorNames = authorsString.split(',');

    const authors: Author[] = authorNames.map((authorName, index) => {
      const nameParts = authorName.trim().split(' ');

      if (nameParts.length === 2) {
        const [name, surname] = nameParts;
        return { name, surname };
      }

      if (nameParts.length === 3) {
        const [name, middleName, surname] = nameParts;
        return { name, middleName, surname };
      }

      return { name: '', middleName: '', surname: '' };
    });

    return authors;
  }

  mapResponse(response: Book): Book {
    var formattedDate;
    if (response.publicationDate != undefined) {
      formattedDate = new Date(response.publicationDate).toISOString().split('T')[0];
    }

    if (response.booksAuthors != undefined && response.booksAuthors?.length > 0) {
      for (let i = 0; i < response.booksAuthors.length; i++) {
        if (i > 0) {
          this.authorsString += ", ";
        }
        this.authorsString += response.booksAuthors[i].name + " "
        if (response.booksAuthors[i].middleName != null) {
          this.authorsString += response.booksAuthors[i].middleName + " ";
        }
        this.authorsString += response.booksAuthors[i].surname;
      }
    }

    this.book = {
      bookId: response.bookId,
      title: response.title,
      description: response.description,
      quantityInStock: response.quantityInStock,
      genre: response.genre,
      price: response.price,
      publisher: response.publisher,
      numberOfPages: response.numberOfPages,
      bookFormat: response.bookFormat,
      publicationDate: formattedDate,
      bookLanguage: response.bookLanguage,
      booksAuthors: response.booksAuthors,
      coverImageURL: response.coverImageURL
    };

    return this.book;
  }

}
