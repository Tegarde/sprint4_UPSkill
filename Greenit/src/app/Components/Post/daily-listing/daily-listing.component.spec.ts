import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyListingComponent } from './daily-listing.component';

describe('DailyListingComponent', () => {
  let component: DailyListingComponent;
  let fixture: ComponentFixture<DailyListingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DailyListingComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DailyListingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
