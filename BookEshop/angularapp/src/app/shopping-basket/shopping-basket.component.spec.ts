import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShoppingBasketComponent } from './shopping-basket.component';

describe('ShoppingBasketComponent', () => {
  let component: ShoppingBasketComponent;
  let fixture: ComponentFixture<ShoppingBasketComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ShoppingBasketComponent]
    });
    fixture = TestBed.createComponent(ShoppingBasketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
