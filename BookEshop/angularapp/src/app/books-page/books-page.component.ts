import { Component } from '@angular/core';
import { Book } from '../interfaces/book.model';
import { BookPageService } from '../services/book-page.service';

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

  constructor(private bookPageService: BookPageService) { }

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
}
