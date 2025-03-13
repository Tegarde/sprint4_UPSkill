import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommentService } from '../../Services/comment.service';
import { SignInService } from '../../Services/sign-in.service';
import { TokenInfo } from '../../Models/token-info';

@Component({
  selector: 'app-like-comment',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './like-comment.component.html',
  styleUrl: './like-comment.component.css'
})
export class LikeCommentComponent implements OnInit {
  /** The ID of the comment to be liked/unliked */
  @Input() commentId!: number;

  /** The username of the authenticated user */
  username?: string;

  /** Stores authenticated user information */
  user: TokenInfo | null = null;

  /** Indicates if the user has liked the comment */
  isCommentLiked = false;

  /** Stores the total number of likes for the comment */
  likesCount: number = 0;

  /**
   * Constructor initializes required services
   * @param commentService - Service for handling comment interactions
   * @param authService - Service for managing user authentication
   */
  constructor(private commentService: CommentService, private authService: SignInService) {}

  /**
   * Lifecycle hook that runs when the component initializes
   * - Retrieves user information
   * - Loads the like count and user like status
   */
  ngOnInit(): void {
    this.authService.getUserSubject().subscribe({
      next: (user) => {
        this.user = user;
        this.username = user!.username;
        this.getLikesCount();
        this.getLikeFromUser();
      }
    });
  }

  /**
   * Retrieves the total number of likes for the comment
   */
  getLikesCount(): void {
    this.commentService.getCommentLikeCount(this.commentId).subscribe({
      next: (likes) => {
        this.likesCount = likes.likesCount;
      }
    });
  }

  /**
   * Checks if the authenticated user has liked the comment
   */
  getLikeFromUser(): void {
    this.commentService.getCommentLikeByUsername(this.commentId, this.username!).subscribe({
      next : (answer) => {
        if (answer == 0) {
          this.isCommentLiked = false;
        }
        if (answer == 1) {
          this.isCommentLiked = true;
        }
      }
    });
  }

  /**
   * Toggles the like status for the comment
   * - If already liked, removes the like
   * - If not liked, adds a like
   */
  toggleLikeInComment(): void {
    if (this.isCommentLiked) {
      this.commentService.unlikeAComment(this.commentId, this.username!).subscribe({
        next: () => {
          this.isCommentLiked = false;
          this.likesCount--;
        }
      });
    } else {
      this.commentService.likeAComment(this.commentId, this.username!).subscribe({
        next: () => {
          this.isCommentLiked = true;
          this.likesCount++;
        }
      });
    }
  }
}