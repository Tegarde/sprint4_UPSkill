import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PostService } from '../../Services/post.service';
import { SignInService } from '../../Services/sign-in.service';

@Component({
  selector: 'app-like-dislike',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './like-dislike.component.html',
  styleUrl: './like-dislike.component.css'
})
export class LikeDislikeComponent implements OnInit {
  @Input() postId! : number

  username? : string;

  likesCount : number = 0;
  dislikesCount : number = 0;

  isLiked = false;
  isDisliked = false;

  constructor(private postService : PostService, private authService : SignInService) { }

  ngOnInit(): void {
    this.getPostInteractions();
    this.authService.getUserSubject().subscribe({
      next : (user) => {
        this.username = user!.username;
        this.getUserInteractionsOnPost();
      }
    });
    
  }

  toggleLike() {
    if (this.isLiked) {
      this.unlikeAPost();
    } else {
      this.likeAPost();
    }
  }

  toggleDislike() {
    if (this.isDisliked) {
      this.undislikeAPost();
    } else {
      this.dislikeAPost();
    }
  }

  likeAPost() {
    this.postService.likeAPost(this.postId, this.username!).subscribe({
      next : () => {
        this.likesCount ++;
        if (this.isDisliked) {
          this.isDisliked = false;
          this.dislikesCount --;
        }
      }
    });
  }

  dislikeAPost() {
    this.postService.dislikeAPost(this.postId, this.username!).subscribe({
      next : () => {
        this.dislikesCount ++;
        if (this.isLiked) {
          this.isLiked = false;
          this.likesCount --;
        }
        
      }
    });
  }

  unlikeAPost() {
    this.postService.unlikeAPost(this.postId, this.username!).subscribe({
      next : () => {
        this.likesCount --;
      }
    });
  }

  undislikeAPost() {
    this.postService.undislikeAPost(this.postId, this.username!).subscribe({
      next : () => {
        this.dislikesCount --;
      }
    });
  } 


  
  getPostInteractions() {
    this.postService.getPostInteractions(this.postId).subscribe({
      next : (interactions) => {
        this.likesCount = interactions.likes;
        this.dislikesCount = interactions.dislikes;
      }
    });
  }
  getUserInteractionsOnPost() {
    this.postService.getInteractionByUser(this.postId, this.username!).subscribe({
      next : (interaction) => {
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
