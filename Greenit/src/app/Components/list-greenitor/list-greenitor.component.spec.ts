import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListGreenitorComponent } from './list-greenitor.component';

describe('ListGreenitorComponent', () => {
  let component: ListGreenitorComponent;
  let fixture: ComponentFixture<ListGreenitorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListGreenitorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ListGreenitorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
