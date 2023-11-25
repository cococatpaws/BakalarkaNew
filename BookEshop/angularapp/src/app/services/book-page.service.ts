import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, tap } from 'rxjs';
import { Book } from '../interfaces/book.model';

@Injectable({
  providedIn: 'root',
})
export class BookPageService {
  private apiUrl = 'https://localhost:7073'; // to kde mi bezi backend

  constructor(private http: HttpClient) { }

  getAllBooksWithAuthors(): Observable<Book[]> {
    return this.http.get<any>(`${this.apiUrl}/knihy`).pipe(
      map(response => {
        if (response && response.value && response.value.$values) {
          return response.value.$values.map((book: Book) => ({
            bookId: book.bookId,
            title: book.title,
            price: book.price,
            coverImageURL: book.coverImageURL
          }));
        } else {
          return [];
        }
      })
    );
  }

  saveBookInDB(data: Book): Observable<Book> {
    return this.http.post<Book>(`${this.apiUrl}/saveBook`, data);
  }

  deleteBook(bookId: number) {
    return this.http.delete<number>(`${this.apiUrl}/knihy`, { body: bookId });
  }
}
