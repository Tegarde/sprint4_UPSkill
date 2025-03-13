import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PostService } from '../../Services/post.service';
import { SignInService } from '../../Services/sign-in.service';
import { TokenInfo } from '../../Models/token-info';

@Component({
  selector: 'app-like-dislike',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './like-dislike.component.html',
  styleUrl: './like-dislike.component.css'
})
export class LikeDislikeComponent implements OnInit {
  /** The ID of the post to like/dislike */
  @Input() postId!: number;

  /** The authenticated user's username */
  username?: string;

  /** Stores authenticated user information */
  user: TokenInfo | null = null;

  /** Number of likes for the post */
  likesCount: number = 0;

  /** Number of dislikes for the post */
  dislikesCount: number = 0;

  /** Indicates whether the user has liked the post */
  isLiked = false;

  /** Indicates whether the user has disliked the post */
  isDisliked = false;

  /**
   * Constructor initializes required services
   * @param postService - Service for handling post interactions
   * @param authService - Service for managing user authentication
   */
  constructor(private postService: PostService, private authService: SignInService) {}

  /**
   * Lifecycle hook that runs when the component initializes
   * - Retrieves the post's total interactions
   * - Retrieves user-specific interaction data
   */
  ngOnInit(): void {
    this.getPostInteractions();
    this.authService.getUserSubject().subscribe({
      next: (user) => {
        this.user = user;
        this.username = user!.username;
        this.getUserInteractionsOnPost();
      }
    });
  }

  /**
   * Toggles the like status for the post
   * - If already liked, removes the like
   * - If not liked, adds a like
   */
  toggleLike(): void {
    if (this.isLiked) {
      this.unlikeAPost();
    } else {
      this.likeAPost();
    }
  }

  /**
   * Toggles the dislike status for the post
   * - If already disliked, removes the dislike
   * - If not disliked, adds a dislike
   */
  toggleDislike(): void {
    if (this.isDisliked) {
      this.undislikeAPost();
    } else {
      this.dislikeAPost();
    }
  }

  /**
   * Sends a request to like the post
   * - Increments likes count
   * - Removes dislike if previously disliked
   */
  likeAPost(): void {
    this.postService.likeAPost(this.postId, this.username!).subscribe({
      next: () => {
        this.likesCount++;
        if (this.isDisliked) {
          this.isDisliked = false;
          this.dislikesCount--;
        }
      }
    });
  }

  /**
   * Sends a request to dislike the post
   * - Increments dislikes count
   * - Removes like if previously liked
   */
  dislikeAPost(): void {
    this.postService.dislikeAPost(this.postId, this.username!).subscribe({
      next: () => {
        this.dislikesCount++;
        if (this.isLiked) {
          this.isLiked = false;
          this.likesCount--;
        }
      }
    });
  }

  /**
   * Sends a request to remove the like from the post
   * - Decrements likes count
   */
  unlikeAPost(): void {
    this.postService.unlikeAPost(this.postId, this.username!).subscribe({
      next: () => {
        this.likesCount--;
      }
    });
  }

  /**
   * Sends a request to remove the dislike from the post
   * - Decrements dislikes count
   */
  undislikeAPost(): void {
    this.postService.undislikeAPost(this.postId, this.username!).subscribe({
      next: () => {
        this.dislikesCount--;
      }
    });
  }

  /**
   * Retrieves the total number of likes and dislikes for the post
   */
  getPostInteractions(): void {
    this.postService.getPostInteractions(this.postId).subscribe({
      next: (interactions) => {
        this.likesCount = interactions.likes;
        this.dislikesCount = interactions.dislikes;
      }
    });
  }

  /**
   * Retrieves the user's interaction with the post
   * - Updates `isLiked` or `isDisliked` based on response
   */
  getUserInteractionsOnPost(): void {
    this.postService.getInteractionByUser(this.postId, this.username!).subscribe({
      next: (interaction) => {
        if (interaction.interaction == '1') {
          this.isLiked = true;
          this.isDisliked = false;
        } else if (interaction.interaction == '-1') {
          this.isDisliked = true;
          this.isLiked = false;
        }
      }
    });
  }
}