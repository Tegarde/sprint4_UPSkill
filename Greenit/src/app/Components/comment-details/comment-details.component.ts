import { DatePipe, NgFor, NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Comment } from '../../Models/comment';
import { LikeCommentComponent } from "../like-comment/like-comment.component";
import { CommentService } from '../../Services/comment.service';
import { MakeCommentComponent } from '../make-comment/make-comment.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-comment-details',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, CommentDetailsComponent, LikeCommentComponent, MakeCommentComponent, RouterLink],
  templateUrl: './comment-details.component.html',
  styleUrl: './comment-details.component.css'
})
export class CommentDetailsComponent {
  @Input() comment! : Comment;

  makingAComment : boolean = false;

  constructor(private commentService : CommentService) { }

  replies? : Comment[];

  showReplies() {
    this.commentService.getCommentsFromComment(this.comment.id).subscribe({
      next : (replies) => {
        this.replies = replies;
      }
    })
  }

  toggleMakingAComment() {
    this.makingAComment = !this.makingAComment;
  }
}
