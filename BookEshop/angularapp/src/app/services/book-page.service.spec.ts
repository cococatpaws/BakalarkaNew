import { TestBed } from '@angular/core/testing';

import { BookPageService } from './book-page.service';

describe('BookPageService', () => {
  let service: BookPageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BookPageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
