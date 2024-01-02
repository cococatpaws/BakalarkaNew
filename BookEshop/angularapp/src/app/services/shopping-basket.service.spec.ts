import { TestBed } from '@angular/core/testing';

import { ShoppingBasketService } from './shopping-basket.service';

describe('LocalStorageService', () => {
  let service: ShoppingBasketService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ShoppingBasketService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
