import { Component, OnInit } from '@angular/core';
import { Post } from '../../../Models/post';
import { CommonModule, DatePipe, NgFor, NgIf } from '@angular/common';
import { CommentDetailsComponent } from '../../comment-details/comment-details.component';
import { PostService } from '../../../Services/post.service';
import { LikeDislikeComponent } from '../../like-dislike/like-dislike.component';
import { MakeCommentComponent } from "../../make-comment/make-comment.component";
import { ActivatedRoute, RouterLink } from '@angular/router';
import { SignInService } from '../../../Services/sign-in.service';
import { FavoriteButtonComponent } from "../../favorite-button/favorite-button.component";
import { TokenInfo } from '../../../Models/token-info';
import { UpdatePost } from '../../../Models/update-post';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-post-details',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, CommentDetailsComponent, LikeDislikeComponent, MakeCommentComponent, FavoriteButtonComponent, RouterLink,FormsModule],
  templateUrl: './post-details.component.html',
  styleUrl: './post-details.component.css'
})
export class PostDetailsComponent implements OnInit {
  post? : Post;

  makingAComment = false;

  user : TokenInfo | null = null;

  editing = false;

  editedPost : UpdatePost = { id: 0, title: '', content: '', createdBy: ''}

  constructor(private authService : SignInService, private postService : PostService, private route : ActivatedRoute) { }

  ngOnInit(): void {
    this.authService.getUserSubject().subscribe({
      next : (user) => {
        this.user = user;
      }
    })

    this.route.queryParams.subscribe({
      next : (params) => {
        this.postService.getPostById(params['id']).subscribe({
          next : (post) => {
            this.post = post;
            this.editedPost = { id: post.id, title: post.title, content: post.content, createdBy: post.createdBy };
            this.authService.getUserSubject().subscribe({
              next : (user) => {
                if (user != null && user.username == post.createdBy) {
                  this.postService.resetPostInteractions(post.id).subscribe();
                }
              }
            })
          },
          error : (error) => {
            console.log(error);
          }
        });
      }
    })
  }

  toggleMakingAComment() {
    this.makingAComment = !this.makingAComment;
  }

  updatePost() {
    if (!this.post) return;

    this.postService.updatePost(this.post.id, this.editedPost).subscribe({
      next: (updatedPost) => {
        console.log("Post atualizado com sucesso!", updatedPost);
        this.post = updatedPost; // Atualiza os dados no frontend
        this.editing = false; // Fecha o modo de edição
      },
      error: (error) => console.error("Erro ao atualizar o post:", error)
    });
  }

  toggleEditing() {
    this.editing = !this.editing;
    if (this.editing && this.post) {
      this.editedPost = {id: this.post.id, title: this.post.title, content: this.post.content, createdBy: this.post.createdBy}; // Restaura os dados antes da edição
    }
  }
}
