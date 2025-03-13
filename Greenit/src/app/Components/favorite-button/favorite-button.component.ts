import { Component, Input, OnInit } from '@angular/core';
import { PostService } from '../../Services/post.service';
import { SignInService } from '../../Services/sign-in.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-favorite-button',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './favorite-button.component.html',
  styleUrl: './favorite-button.component.css'
})
export class FavoriteButtonComponent implements OnInit {
  /** The ID of the post to be marked as favorite */
  @Input() postId!: number;

  /** Indicates whether the post is favorited by the user */
  isFavorite: boolean = false;

  /** Stores the username of the authenticated user */
  username?: string;

  /**
   * Constructor initializes required services
   * @param postService - Service for handling post interactions
   * @param authService - Service for managing user authentication
   */
  constructor(private postService: PostService, private authService: SignInService) {}

  /**
   * Lifecycle hook that runs when the component initializes
   * - Retrieves authenticated user and checks if the post is favorited
   */
  ngOnInit(): void {
    this.authService.getUserSubject().subscribe({
      next: (user) => {
        this.username = user!.username;
        this.postService.isPostFavorite(this.postId, this.username).subscribe({
          next : (isFavorite) => this.isFavorite = ((isFavorite.favorite == 0) ? false : true)
        });
      }
    });
  }

  /**
   * Toggles the favorite status of the post
   * - If already favorited, removes it from favorites
   * - If not favorited, adds it to favorites
   */
  toggleFavorite(): void {
    if (this.isFavorite) {
      this.postService.removeFavoritePost(this.postId, this.username!).subscribe({
        next: () => this.isFavorite = false
      });
    } else {
      this.postService.addFavoritePost(this.postId, this.username!).subscribe({
        next: () => this.isFavorite = true
      });
    }
  }
}