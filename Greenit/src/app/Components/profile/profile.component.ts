import { CommonModule, NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { SignUpService } from '../../Services/sign-up.service';
import { GreenitorComplete } from '../../Models/greenitor-complete';
import { Post } from '../../Models/post';
import { PostService } from '../../Services/post.service';
import { PostListingComponent } from '../Post/post-listing/post-listing.component';
import { GreenitorService } from '../../Services/greenitor.service';
import { SignInService } from '../../Services/sign-in.service';
import { ActivatedRoute } from '@angular/router';
import { TokenInfo } from '../../Models/token-info';
import { ProfileUpdateComponent } from "../profile-update/profile-update.component";

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, PostListingComponent, NgFor, ProfileUpdateComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  user? : GreenitorComplete;
  posts: Post[] = [];

  favoritePosts: Post[] = [];

  username? : string

  loggedUser : TokenInfo | null = null;

  stats = {
    likesInPosts: 0,
    dislikesInPosts: 0,
    likesInComments: 0,
    eventAttendances: 0,
    posts: 0,
    comments: 0,
    favoritePosts: 0
  }
  
  constructor(private authService : SignInService, private userService: SignUpService, private route : ActivatedRoute, private postService: PostService, private greenitorService: GreenitorService) { }
  
  

  ngOnInit() {
    this.route.params.subscribe({
      next: (params) => {
        this.username = params['username'];
        this.getUserByUsername();
        this.getPostsFromUser();
        this.getStats();
        this.getFavoritePosts();
        this.authService.getUserSubject().subscribe(u => this.loggedUser = u);
      }
    })
  }

  getFavoritePosts() {
    this.postService.getFavoritePosts(this.username!).subscribe({
      next : (posts) => {
        this.favoritePosts = posts;
      },
      error : (err) => console.error("Error fetching favorite posts")
    })
  }

  getUserByUsername() {
    this.userService.getUserByUsername(this.username!).subscribe({
      next: (user) => {
        this.user = user;
      },
      error: (err) => console.error("Error fetching user:", err)
    });
  }

  getPostsFromUser() {
    this.postService.getPostsByUser(this.username!).subscribe({
      next: (posts) => {
        this.posts = posts;
        console.log("Posts:", posts);
      },
      error: (err) => console.error("Error fetching posts:", err)
    });
  }

  getStats() {
    this.greenitorService.getGreenitorStats(this.username!).subscribe({
      next: (stats) => {this.stats = stats;

      },
      error: (err) => console.error("Error fetching stats:", err)
    });
  }


}


