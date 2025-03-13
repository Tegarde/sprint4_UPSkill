import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommentService } from '../../Services/comment.service';
import { SignInService } from '../../Services/sign-in.service';

@Component({
  selector: 'app-make-comment',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './make-comment.component.html',
  styleUrl: './make-comment.component.css'
})
export class MakeCommentComponent implements OnInit {
  /** The ID of the post, event, or comment that the comment is related to */
  @Input() id!: number;

  /** The type of entity being commented on ('post', 'event', or 'comment') */
  @Input() type!: string;

  /** Stores the authenticated user's username */
  username: string = '';

  /** The content of the comment to be submitted */
  content: string = '';

  /**
   * Constructor initializes required services
   * @param commentService - Service for handling comment-related API calls
   * @param authService - Service for managing user authentication
   */
  constructor(private commentService: CommentService, private authService: SignInService) {}

  /**
   * Lifecycle hook that runs when the component initializes
   * - Retrieves the authenticated user's username
   */
  ngOnInit(): void {
    this.authService.getUserSubject().subscribe((username) => {
      this.username = username!.username;
    });
  }

  /**
   * Submits a comment based on the type (post, event, or comment)
   * - Calls the corresponding API method and reloads the page upon success
   */
  submitComment(): void {
    switch (this.type) {
      case 'post':
        this.commentService.commentAPost({
          postId: this.id,
          content: this.content,
          createdBy: this.username
        }).subscribe(() => {
          window.location.reload();
        });
        break;
      case 'event':
        this.commentService.commentAnEvent({
          eventId: this.id,
          content: this.content,
          createdBy: this.username
        }).subscribe(() => {
          window.location.reload();
        });
        break;
      case 'comment':
        this.commentService.commentAComment({
          parentCommentId: this.id,
          content: this.content,
          createdBy: this.username
        }).subscribe(() => {
          window.location.reload();
        });
        break;
    }
  }
}