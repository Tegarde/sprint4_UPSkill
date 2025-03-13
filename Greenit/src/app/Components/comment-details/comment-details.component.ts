import { DatePipe, NgFor, NgIf } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { Comment } from '../../Models/comment';
import { LikeCommentComponent } from "../like-comment/like-comment.component";
import { CommentService } from '../../Services/comment.service';
import { MakeCommentComponent } from '../make-comment/make-comment.component';
import { RouterLink } from '@angular/router';
import { SignInService } from '../../Services/sign-in.service';
import { TokenInfo } from '../../Models/token-info';

@Component({
  selector: 'app-comment-details',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, CommentDetailsComponent, LikeCommentComponent, MakeCommentComponent, RouterLink],
  templateUrl: './comment-details.component.html',
  styleUrl: './comment-details.component.css'
})
export class CommentDetailsComponent implements OnInit {
  /** The comment object passed to the component */
  @Input() comment! : Comment;

  /** Flag to control if the user is making a new comment */
  makingAComment : boolean = false;

  /** Holds the authenticated user information */
  user : TokenInfo | null = null;
 
  /** List of replies to the current comment */
  replies? : Comment[];
 
  /**
   * Constructor injecting the CommentService and SignInService
   * @param commentService - Handles comment-related API calls
   * @param authService - Manages user authentication
   */
  constructor(private commentService : CommentService, private authService : SignInService) { }

  /**
   * Lifecycle hook that runs when the component initializes
   * - Subscribes to authentication changes
   */
  ngOnInit(): void {
    this.authService.getUserSubject().subscribe((user) => this.user = user);
  } 

  /**
   * Fetches replies for the current comment from the service
   */
  showReplies() {
    this.commentService.getCommentsFromComment(this.comment.id).subscribe({
      next : (replies) => {
        this.replies = replies;
      }
    })
  }

  /**
   * Toggles the `makingAComment` flag to show or hide the comment input
   */
  toggleMakingAComment() {
    this.makingAComment = !this.makingAComment;
  }
}