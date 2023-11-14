import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidebarShoppingInfoComponent } from './sidebar-shopping-info.component';

describe('SidebarShoppingInfoComponent', () => {
  let component: SidebarShoppingInfoComponent;
  let fixture: ComponentFixture<SidebarShoppingInfoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SidebarShoppingInfoComponent]
    });
    fixture = TestBed.createComponent(SidebarShoppingInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
