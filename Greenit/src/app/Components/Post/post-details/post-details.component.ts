import { Component, OnInit } from '@angular/core';
import { Post } from '../../../Models/post';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { CommentDetailsComponent } from '../../comment-details/comment-details.component';
import { PostService } from '../../../Services/post.service';

@Component({
  selector: 'app-post-details',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, CommentDetailsComponent],
  templateUrl: './post-details.component.html',
  styleUrl: './post-details.component.css'
})
export class PostDetailsComponent implements OnInit {
  post? : Post;

  constructor(private postService : PostService) { }

  ngOnInit(): void {
    this.postService.getPostById(7).subscribe({
      next : (post) => {
        this.post = post;
      },
      error : (error) => {
        console.log(error);
      }
    });
  }

}
