import { Component } from '@angular/core';
import { BookPageService } from '../services/book-page.service';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from '../services/notification.service';
import { Author } from '../interfaces/author.model';
import { Book } from '../interfaces/book.model';
import { ShoppingBasketService } from '../services/shopping-basket.service';
import { ShoppingBasketItem } from '../interfaces/shopping-basket-item';

@Component({
  selector: 'app-book-page',
  templateUrl: './book-page.component.html',
  styleUrls: ['./book-page.component.css']
})
export class BookPageComponent {
  book: Book = {
    bookId: 0,
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
    coverImageURL: ""
  };

  authorsString: string = '';
  bookAuthors: Author[] = [];

  constructor(private bookPageService: BookPageService, private route: ActivatedRoute, private notificationService: NotificationService,
    private shoppingBasketService: ShoppingBasketService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.book.bookId = +params['id'];
    });

    if (this.book.bookId != undefined)
    this.bookPageService.getBookById(this.book.bookId).subscribe(response => {
      this.mapResponse(response);
    }

    );
  }

  onSubmit(): void {
    this.bookAuthors = this.mapBookAuthors(this.authorsString);
    this.book.booksAuthors = this.bookAuthors;

    this.bookPageService.editBookInDB(this.book).subscribe({
      next: (response: Book) => {
        this.notificationService.displayMessage("Informácie o knihe bolo úspešne zmenené!", "success");
        window.location.reload();
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

  addToBasket() {
    var shoppingBasketItem: ShoppingBasketItem = {
      bookId: this.book.bookId!,
      bookTitle: this.book.title,
      bookAuthors: this.book.booksAuthors!,
      bookQuantity: 1,
      price: this.book.price,
      totalPrice: this.book.price,
      imageURL: this.book.coverImageURL
    };

    if (this.book.bookId != undefined)
      this.shoppingBasketService.saveBookWithExpiration(this.book.bookId.toString(), shoppingBasketItem, 24);
  }
}
