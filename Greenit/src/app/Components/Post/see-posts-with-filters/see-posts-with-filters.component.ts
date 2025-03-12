import { AsyncPipe, DatePipe, NgClass, NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Post } from '../../../Models/post';
import { PostService } from '../../../Services/post.service';
import { CategoryService } from '../../../Services/category.service';
import { PostListingComponent } from "../post-listing/post-listing.component";
import { TokenInfo } from '../../../Models/token-info';
import { SignInComponent } from '../../sign-in/sign-in.component';
import { SignInService } from '../../../Services/sign-in.service';

@Component({
  selector: 'app-see-posts-with-filters',
  standalone: true,
  imports: [NgClass, NgIf, NgFor, DatePipe],
  templateUrl: './see-posts-with-filters.component.html',
  styleUrl: './see-posts-with-filters.component.css'
})
export class SeePostsWithFiltersComponent implements OnInit {

  user : TokenInfo | null = null;

  isSideBarOpen = false;

  allPosts : Post[] = [];

  showingPosts : Post[] = [];

  categories : any[] = [];

  constructor(private postService : PostService, private categoryService : CategoryService, private authService : SignInService) {}

  ngOnInit(): void {
    this.categoryService.getCategories().subscribe({
      next : (categories) => {
        this.categories = categories;
      }
    });

    this.postService.getAllPosts().subscribe({
      next : (posts) => {
        this.allPosts = posts.sort((a: Post, b:Post) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        this.authService.getUserSubject().subscribe({
          next : (user) => {
            this.user = user;
            this.setShowingPosts();
          }
        })
      }
    })
  }


  setShowingPosts() {
    if (this.user! !== null && this.user!.role === "Moderator") {
      this.showingPosts = this.allPosts;
    } else {
      this.showingPosts = this.allPosts.filter(p => p.status == true);
    }
  }
}
