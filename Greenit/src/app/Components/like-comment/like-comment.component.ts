import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommentService } from '../../Services/comment.service';
import { SignInService } from '../../Services/sign-in.service';

@Component({
  selector: 'app-like-comment',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './like-comment.component.html',
  styleUrl: './like-comment.component.css'
})
export class LikeCommentComponent implements OnInit {
  @Input() commentId! : number;

  username? : string;

  isCommentLiked = false;

  likesCount : number = 0;

  constructor(private commentService : CommentService, private authService : SignInService) { }

  ngOnInit(): void {
     this.authService.getUserSubject().subscribe({
       next : (user) => {
         this.username = user!.username;
         this.getLikesCount();
         this.getLikeFromUser();
       }
     })
  }

  getLikesCount()  {
    this.commentService.getCommentLikeCount(this.commentId).subscribe({
      next : (likes) => {
        this.likesCount = likes.likesCount;
      }
    });
  }

  getLikeFromUser() {
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

  toggleLikeInComment() {
    if (this.isCommentLiked) {
      this.commentService.unlikeAComment(this.commentId, this.username!).subscribe({
        next : () => {
          this.isCommentLiked = false;
          this.likesCount --;
        }
      });
    } else {
      this.commentService.likeAComment(this.commentId, this.username!).subscribe({
        next : () => {
          this.isCommentLiked = true;
          this.likesCount ++;
        }
      });
    }
  }
}

