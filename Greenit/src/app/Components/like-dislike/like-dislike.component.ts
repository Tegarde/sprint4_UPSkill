import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-like-dislike',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './like-dislike.component.html',
  styleUrl: './like-dislike.component.css'
})
export class LikeDislikeComponent {
  @Input() postId! : number

  likesCount : number = 115
  dislikesCount : number = 5

  isLiked = false;
  isDisliked = false;

  toggleLike() {
    if (this.isDisliked) {
      this.toggleDislike();
      this.isDisliked = false;
    }
    if (this.isLiked) {
      this.likesCount --;
    } else {
      this.likesCount ++;
    }
  }

  toggleDislike() {
    if (this.isLiked) {
      this.toggleLike();
      this.isLiked = false;
    }
    if (this.isDisliked) {
      this.dislikesCount --;
    } else {
      this.dislikesCount ++;
    }
  }  
}
