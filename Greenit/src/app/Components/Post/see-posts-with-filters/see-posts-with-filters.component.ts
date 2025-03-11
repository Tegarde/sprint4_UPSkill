import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-see-posts-with-filters',
  standalone: true,
  imports: [NgClass, NgIf],
  templateUrl: './see-posts-with-filters.component.html',
  styleUrl: './see-posts-with-filters.component.css'
})
export class SeePostsWithFiltersComponent {
  isSideBarOpen = false;
}
