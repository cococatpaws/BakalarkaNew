import { BookFormat } from "./Enums/book-format.enum";
import { BookLanguage } from "./Enums/book-language.enum";
import { Genre } from "./Enums/genre.enum";
import { Author } from "./author.model";

export interface Book {
  bookId?: number;
  title: string;
  description?: string;
  quantityInStock: number;
  coverImageURL: string;
  genre?: Genre;
  price: number;
  publisher?: string;
  numberOfPages?: number;
  bookFormat: BookFormat;
  publicationDate: Date;
  bookLanguage: BookLanguage;
  booksAuthors?: Author[];
}
