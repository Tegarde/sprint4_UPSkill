import { Component, OnInit } from '@angular/core';
import { Post } from '../../../Models/post';
import { PostService } from '../../../Services/post.service';
import { PostListingComponent } from '../post-listing/post-listing.component';

@Component({
  selector: 'app-monthly-listing',
  standalone: true,
  imports: [PostListingComponent],
  templateUrl: './monthly-listing.component.html',
  styleUrl: './monthly-listing.component.css'
})
export class MonthlyListingComponent implements OnInit{
  posts? : Post[]

  constructor(private postService : PostService) { }

  ngOnInit(): void {
    this.postService.getMonthlyTrendingPosts(3).subscribe((posts) => this.posts = posts);
  }
}
