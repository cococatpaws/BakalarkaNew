import { Component, OnInit } from '@angular/core';
import { Book } from '../interfaces/book.model';
import { BookPageService } from '../services/book-page.service';
import { Router } from '@angular/router';
import { NotificationService } from '../services/notification.service';
import { AuthDataService } from '../services/auth-data.service';

@Component({
  selector: 'app-books-page',
  templateUrl: './books-page.component.html',
  styleUrls: ['./books-page.component.css']
})
export class BooksPageComponent {
  barFilters: string[] = ['Predvolené', 'Najlacnejšie', 'Najdrahšie', 'Bestsellery', 'Novinky', 'Hodnotenie', 'Zľavy'];
  languages: string[] = ['Slovenčina', 'Čeština', 'Angličtina', 'Nemčina', 'Ruština'];
  genres: string[] = ['Romantické', 'Fantasy', 'Thriller', 'Náučné', 'Pre deti', 'Detektívky', 'Poézia', 'Klasika'];
  newReleases: string[] = ['Tento mesiac', 'Tento rok']

  role: string = "";

  sidebarShown: boolean = false;

  books: Book[] = [];

  constructor(private bookPageService: BookPageService, private router: Router, private notificationService: NotificationService,
                private authDataService: AuthDataService) { }

  ngOnInit(): void {
    this.getBooks();

    this.authDataService.getRole().subscribe(response => {
      this.role = response
      
    });
    console.log(this.role);
    console.log(sessionStorage);
  }

  getBooks(): void {
    this.bookPageService.getAllBooksWithAuthors().subscribe(
      (books) => {
        this.books = books;
      },
      (error) => {
        console.error('Error fetching books:', error);
      }
    );

    console.log(this.books);
  }

  showSidebar() {
    this.sidebarShown = !this.sidebarShown;
    console.log(this.sidebarShown);
  }

  deleteBook(bookId: number | undefined): void {
    if (bookId != undefined) {
      this.bookPageService.deleteBook(bookId).subscribe({
        next: () => {
          setTimeout(() => {
            window.location.reload();
          }, 500)

          this.notificationService.displayMessage("Kniha bola úspešne odstránená!", "success");
        },
        error: (error) => {
          this.notificationService.displayMessage("Nastala chyba pri odstraňovaní knihy!", "warning");
          console.log(error);
        },
      });
    }
  }

  editBook(bookId: number | undefined): void {
    this.router.navigate(['/uprav-knihu', bookId]);
  }

  displayBook(bookId: number | undefined): void {
    this.router.navigate(['/zobraz-knihu', bookId]);
  }
}
