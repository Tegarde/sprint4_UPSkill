import { TestBed } from '@angular/core/testing';

import { GreenitorService } from './greenitor.service';

describe('GreenitorService', () => {
  let service: GreenitorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GreenitorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
