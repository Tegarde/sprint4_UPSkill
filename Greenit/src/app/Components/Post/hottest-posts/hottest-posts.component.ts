import { Component, OnInit } from '@angular/core';
import { PostService } from '../../../Services/post.service';
import { Post } from '../../../Models/post';
import { PostListingComponent } from '../post-listing/post-listing.component';

@Component({
  selector: 'app-hottest-posts',
  standalone: true,
  imports: [PostListingComponent],
  templateUrl: './hottest-posts.component.html',
  styleUrl: './hottest-posts.component.css'
})
export class HottestPostsComponent implements OnInit {
  posts? : Post[]
  
    constructor(private postService : PostService) { }
  
    ngOnInit(): void {
      this.postService.getHottestPosts(3).subscribe((posts) => this.posts = posts);
    }
}
