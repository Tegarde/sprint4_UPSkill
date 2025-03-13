import { CommonModule, NgFor } from '@angular/common';
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
  /** Stores user profile information */
  user?: GreenitorComplete;

  /** List of posts created by the user */
  posts: Post[] = [];

  /** List of user's favorite posts */
  favoritePosts: Post[] = [];

  /** Username of the profile being viewed */
  username?: string;

  /** Logged-in user information */
  loggedUser: TokenInfo | null = null;

  /** Stores the user's statistics */
  stats = {
    likesInPosts: 0,
    dislikesInPosts: 0,
    likesInComments: 0,
    eventAttendances: 0,
    posts: 0,
    comments: 0,
    favoritePosts: 0
  };
  
  /**
   * Constructor initializes required services
   * @param authService - Service for managing user authentication
   * @param userService - Service for retrieving user details
   * @param route - ActivatedRoute for accessing route parameters
   * @param postService - Service for fetching posts
   * @param greenitorService - Service for retrieving user statistics
   */
  constructor(
    private authService: SignInService, 
    private userService: SignUpService, 
    private route: ActivatedRoute, 
    private postService: PostService, 
    private greenitorService: GreenitorService
  ) {}

  /**
   * Lifecycle hook that runs when the component initializes
   * - Retrieves username from route parameters
   * - Fetches user details, posts, stats, and favorite posts
   * - Retrieves logged-in user data
   */
  ngOnInit(): void {
    this.route.params.subscribe({
      next: (params) => {
        this.username = params['username'];
        this.getUserByUsername();
        this.getPostsFromUser();
        this.getStats();
        this.getFavoritePosts();
        this.authService.getUserSubject().subscribe(u => this.loggedUser = u);
      }
    });
  }

  /**
   * Fetches the user's favorite posts
   */
  getFavoritePosts(): void {
    this.postService.getFavoritePosts(this.username!).subscribe({
      next: (posts) => {
        this.favoritePosts = posts;
      },
      error: (err) => console.error("Error fetching favorite posts")
    });
  }

  /**
   * Retrieves user details by username
   */
  getUserByUsername(): void {
    this.userService.getUserByUsername(this.username!).subscribe({
      next: (user) => {
        this.user = user;
      },
      error: (err) => console.error("Error fetching user:", err)
    });
  }

  /**
   * Fetches all posts created by the user
   */
  getPostsFromUser(): void {
    this.postService.getPostsByUser(this.username!).subscribe({
      next: (posts) => {
        this.posts = posts;
        console.log("Posts:", posts);
      },
      error: (err) => console.error("Error fetching posts:", err)
    });
  }

  /**
   * Retrieves user statistics related to posts, comments, and event attendance
   */
  getStats(): void {
    this.greenitorService.getGreenitorStats(this.username!).subscribe({
      next: (stats) => {
        this.stats = stats;
      },
      error: (err) => console.error("Error fetching stats:", err)
    });
  }
}