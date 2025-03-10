import { Component } from '@angular/core';
import { Post } from '../../../Models/post';
import { PostService } from '../../../Services/post.service';
import { PostListingComponent } from '../post-listing/post-listing.component';

@Component({
  selector: 'app-daily-listing',
  standalone: true,
  imports: [PostListingComponent],
  templateUrl: './daily-listing.component.html',
  styleUrl: './daily-listing.component.css'
})
export class DailyListingComponent {
  posts? : Post[]
  
    constructor(private postService : PostService) { }
  
    ngOnInit(): void {
      this.postService.getDailyTrendingPosts(3).subscribe((posts) => this.posts = posts);
    }
}
