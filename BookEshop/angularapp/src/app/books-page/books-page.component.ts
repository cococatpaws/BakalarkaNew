import { Component } from '@angular/core';
import { Book } from '../interfaces/book.model';
import { BookPageService } from '../services/book-page.service';
import { Router } from '@angular/router';

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

  sidebarShown: boolean = false;

  books: Book[] = [];

  constructor(private bookPageService: BookPageService, private router: Router) { }

  ngOnInit(): void {
    this.getBooks();
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
          console.log(`Kniha s ID ${bookId} bola úspešne odstránená.`);
          window.location.reload();
        },
        error: (error) => {
          console.error(`Chyba pri odstraňovaní knihy s ID ${bookId}:`, error);
        },
      });
    }
  }

  editBook(bookId: number | undefined): void {
    this.router.navigate(['/uprav-knihu', bookId]);
  }
}
