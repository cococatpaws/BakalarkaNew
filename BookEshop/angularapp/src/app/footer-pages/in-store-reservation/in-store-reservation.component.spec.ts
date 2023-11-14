import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InStoreReservationComponent } from './in-store-reservation.component';

describe('InStoreReservationComponent', () => {
  let component: InStoreReservationComponent;
  let fixture: ComponentFixture<InStoreReservationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InStoreReservationComponent]
    });
    fixture = TestBed.createComponent(InStoreReservationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
