import { Component } from '@angular/core';
import { Post } from '../../Models/post';
import { ActivatedRoute } from '@angular/router';
import { PostService } from '../../Services/post.service';
import { PostListingComponent } from '../Post/post-listing/post-listing.component';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [PostListingComponent],
  templateUrl: './search.component.html',
  styleUrl: './search.component.css'
})
export class SearchComponent {
  /** The search query entered by the user */
  query?: string;

  /** List of posts matching the search query */
  posts?: Post[];

  /**
   * Constructor initializes required services
   * @param route - ActivatedRoute for accessing route parameters
   * @param postService - Service for fetching posts based on search query
   */
  constructor(private route: ActivatedRoute, private postService: PostService) {}

  /**
   * Lifecycle hook that runs when the component initializes
   * - Retrieves the search query from route parameters
   * - Calls the service to fetch posts based on the search query
   */
  ngOnInit(): void {
    this.route.params.subscribe({
      next: (params) => {
        this.query = params['query'];
        this.postService.searchPosts(this.query!).subscribe({
          next: (posts) => {
            this.posts = posts;
          },
          error: (error) => {
            console.log(error);
          }
        });
      }
    });
  }
}