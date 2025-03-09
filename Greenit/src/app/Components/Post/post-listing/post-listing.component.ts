import { Component, Input } from '@angular/core';
import { Post } from '../../../Models/post';
import { DatePipe, NgFor } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-post-listing',
  standalone: true,
  imports: [NgFor, RouterLink, DatePipe],
  templateUrl: './post-listing.component.html',
  styleUrl: './post-listing.component.css'
})
export class PostListingComponent {
  @Input() posts! : Post[];
}
