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

  query? : string;

  posts? : Post[];

  constructor(private route : ActivatedRoute, private postService : PostService) { }

  ngOnInit(): void {
    this.route.params.subscribe({
      next : (params) => {
        this.query = params['query'];
        this.postService.searchPosts(this.query!).subscribe({
          next : (posts) => {
            this.posts = posts;
          },
          error : (error) => {
            console.log(error);
          }
        });
      }
    });
    
  }
}
