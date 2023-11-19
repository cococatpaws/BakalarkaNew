import { Author } from "./author.model";

export interface Book {
  bookId: number;
  title: string;
  description?: string;
  quantityInStock: number;
  coverImageURL: string;
  genre?: string;
  price: number;
  publisher?: string;
  numberOfPages?: number;
  bookFormat: string;
  publicationDate: Date;
  bookLanguage: string;
  booksAuthors?: Author[];
}
