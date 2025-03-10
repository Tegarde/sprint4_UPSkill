import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthlyListingComponent } from './monthly-listing.component';

describe('MonthlyListingComponent', () => {
  let component: MonthlyListingComponent;
  let fixture: ComponentFixture<MonthlyListingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MonthlyListingComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MonthlyListingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
