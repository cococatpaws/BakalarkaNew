import { Author } from "./author.model";

export interface ShoppingBasketItem {
  bookId: number;
  bookTitle: string;
  bookAuthors: Author[];
  bookQuantity: number;
  price: number;
  totalPrice: number;
  imageURL: string;
}
