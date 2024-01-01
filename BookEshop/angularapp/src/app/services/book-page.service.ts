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

  saveBookInDB(data: Book): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/saveBook`, data);
  }

  saveBookPictureInDB(data: FormData) {
    return this.http.post<any>(`${this.apiUrl}/saveBookCover`, data);
  }

  deleteBook(bookId: number) {
    return this.http.delete<number>(`${this.apiUrl}/knihy`, { body: bookId });
  }

  getBookById(bookId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/edit-book/${bookId}`).pipe(
      map(response => {
        if (response && response.value) {
          return {
            bookId: response.value.bookId,
            title: response.value.title,
            description: response.value.description,
            quantityInStock: response.value.quantityInStock,
            coverImageURL: response.value.coverImageURL,
            genre: response.value.genre,
            price: response.value.price,
            publisher: response.value.publisher,
            numberOfPages: response.value.numberOfPages,
            bookFormat: response.value.bookFormat,
            publicationDate: new Date(response.value.publicationDate),
            bookLanguage: response.value.bookLanguage,
            booksAuthors: response.value.booksAuthors.$values.map((authorInfo: any) => ({
              authorId: authorInfo.author.authorId,
              name: authorInfo.author.name,
              middleName: authorInfo.author.middleName,
              surname: authorInfo.author.surname
            }))
          };
        } else {
          return null;
        }
      })
    );
  }

  editBookInDB(updatedBook: Book): Observable<Book> {
    return this.http.put<Book>(`${this.apiUrl}/edit-book/${updatedBook.bookId}`, updatedBook);
  }
 }
