import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-like-comment',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './like-comment.component.html',
  styleUrl: './like-comment.component.css'
})
export class LikeCommentComponent {
  @Input() commentId! : number;

  isCommentLiked = false;

  likesCount : number = 0;



  toggleLikeInComment() {
    this.likesCount ++;
  }
}
