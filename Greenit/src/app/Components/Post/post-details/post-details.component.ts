import { Component, OnInit } from '@angular/core';
import { Post } from '../../../Models/post';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { CommentDetailsComponent } from '../../comment-details/comment-details.component';
import { PostService } from '../../../Services/post.service';
import { LikeDislikeComponent } from '../../like-dislike/like-dislike.component';
import { MakeCommentComponent } from "../../make-comment/make-comment.component";
import { ActivatedRoute } from '@angular/router';
import { SignInService } from '../../../Services/sign-in.service';

@Component({
  selector: 'app-post-details',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, CommentDetailsComponent, LikeDislikeComponent, MakeCommentComponent],
  templateUrl: './post-details.component.html',
  styleUrl: './post-details.component.css'
})
export class PostDetailsComponent implements OnInit {
  post? : Post;

  makingAComment = false;

  constructor(private authService : SignInService, private postService : PostService, private route : ActivatedRoute) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe({
      next : (params) => {
        this.postService.getPostById(params['id']).subscribe({
          next : (post) => {
            this.post = post;
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
}
