import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SeePostsWithFiltersComponent } from './see-posts-with-filters.component';

describe('SeePostsWithFiltersComponent', () => {
  let component: SeePostsWithFiltersComponent;
  let fixture: ComponentFixture<SeePostsWithFiltersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SeePostsWithFiltersComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SeePostsWithFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
