import { BookInOrder } from "./book-in-order";

export interface Order {
  username?: string;
  name: string;
  surname: string;
  email: string;
  phoneNumber: string;
  country: string;
  city: string;
  street: string;
  addressNumber: string;
  postCode: string;
  booksInOrder: BookInOrder[];
  paymentTypeId: number;
  shippingTypeId: number;
  orderType: string;
  orderDetails: string;
}
