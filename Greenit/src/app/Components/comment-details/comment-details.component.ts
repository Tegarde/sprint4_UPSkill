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
  @Input() comment! : Comment;

  makingAComment : boolean = false;

  user : TokenInfo | null = null;
 
  replies? : Comment[];
 
  constructor(private commentService : CommentService, private authService : SignInService) { }

 ngOnInit(): void {
   this.authService.getUserSubject().subscribe((user) => this.user = user);
 } 

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
