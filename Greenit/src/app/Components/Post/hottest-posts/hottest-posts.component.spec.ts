import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HottestPostsComponent } from './hottest-posts.component';

describe('HottestPostsComponent', () => {
  let component: HottestPostsComponent;
  let fixture: ComponentFixture<HottestPostsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HottestPostsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HottestPostsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
