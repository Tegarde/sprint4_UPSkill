import { DatePipe, NgFor, NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Comment } from '../../Models/comment';

@Component({
  selector: 'app-comment-details',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe],
  templateUrl: './comment-details.component.html',
  styleUrl: './comment-details.component.css'
})
export class CommentDetailsComponent {
  @Input() comment! : Comment;

  replies? : Comment[];

  showReplies() {
    this.replies = [
      {
        id : 1,
        content : "Obrigado pelo aviso",
        createdBy : "tegarde",
        createdAt : "2023-06-24",
        postId : 1,
        likedBy : 12,
        commentsCounter : 3
      },
      {
        id : 2,
        content : "Obrigado pelo aviso",
        createdBy : "tegarde",
        createdAt : "2023-06-24",
        postId : 1,
        likedBy : 12,
        commentsCounter : 0
      }
    ]
  }
}
