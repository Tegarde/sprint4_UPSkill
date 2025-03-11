import { Component, Input, OnInit } from '@angular/core';
import { PostService } from '../../Services/post.service';
import { SignInService } from '../../Services/sign-in.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-favorite-button',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './favorite-button.component.html',
  styleUrl: './favorite-button.component.css'
})
export class FavoriteButtonComponent implements OnInit {
  @Input() postId! : number;

  isFavorite : boolean = false;

  username? : string;

  constructor(private postService : PostService, private authService : SignInService) { }

  ngOnInit(): void {
    this.authService.getUserSubject().subscribe({
      next : (user) => {
        this.username = user!.username;
        this.postService.isPostFavorite(this.postId, this.username).subscribe({
          next : (isFavorite) => this.isFavorite = ((isFavorite.favorite == 0) ? false : true)
        });
      }
    })
  }

  toggleFavorite() : void {
    if(this.isFavorite) {
      this.postService.removeFavoritePost(this.postId, this.username!).subscribe({
        next : () => this.isFavorite = false
      });
    } else {
      this.postService.addFavoritePost(this.postId, this.username!).subscribe({
        next : () => this.isFavorite = true
      });
    }
  }
}
