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
        // Check if 'value' property exists and has the '$values' property
        console.log(response.value.$values[0])
        if (response && response.value && response.value.$values) {
          // Map the books to your Book model
          console.log(response.value.$values[0].bookId);
          return response.value.$values.map((book: Book) => ({
            bookId: book.bookId,
            title: book.title,
            price: book.price,
            coverImageURL: book.coverImageURL
            // Map other properties as needed
          }));
        } else {
          // If the structure is not as expected, return an empty array or handle accordingly
          return [];
        }
      })
    );

    /*return this.http.get<any>(`${this.apiUrl}/knihy`).pipe(
      tap(response => console.log('API Response:', response))
    );
/*      .pipe(
      map(response => {
        // Check if 'value' property exists and has the '$values' property
        if (response && response.value && response.value.$values) {
          // Map the books to your Book model
          console.log(response.value);
          return response.value.$values.map((book: Book) => ({
            bookId: book.bookId,
            title: book.title,
            description: book.description,
            // Map other properties as needed
          }));
        } else {
          // If the structure is not as expected, return an empty array or handle accordingly
          return [];
        }
      })
    );*/
  }
}
