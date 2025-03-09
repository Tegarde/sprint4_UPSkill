import { Component, Input } from '@angular/core';
import { Post } from '../../../Models/post';

@Component({
  selector: 'app-post-listing',
  standalone: true,
  imports: [],
  templateUrl: './post-listing.component.html',
  styleUrl: './post-listing.component.css'
})
export class PostListingComponent {
  @Input() posts! : Post[];
}
