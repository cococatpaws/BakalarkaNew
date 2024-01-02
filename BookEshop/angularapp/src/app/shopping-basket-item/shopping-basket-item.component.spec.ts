import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShoppingBasketItemComponent } from './shopping-basket-item.component';

describe('ShoppingBasketItemComponent', () => {
  let component: ShoppingBasketItemComponent;
  let fixture: ComponentFixture<ShoppingBasketItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ShoppingBasketItemComponent]
    });
    fixture = TestBed.createComponent(ShoppingBasketItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
