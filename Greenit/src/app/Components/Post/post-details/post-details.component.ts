import { Component } from '@angular/core';
import { Post } from '../../../Models/post';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { CommentDetailsComponent } from '../../comment-details/comment-details.component';

@Component({
  selector: 'app-post-details',
  standalone: true,
  imports: [NgIf, NgFor, DatePipe, CommentDetailsComponent],
  templateUrl: './post-details.component.html',
  styleUrl: './post-details.component.css'
})
export class PostDetailsComponent {
  post : Post = {
    id : 1,
    title : "Lixo espalhado",
    content : "Tenham cuidado para não deixar muito lixo espalhado",
    createdBy : "tegarde",
    createdAt : "2023-06-24, 12:55:47",
    category : "Ecologia",
    status : true,
    interactions : 0,
    likedBy : 65,
    dislikedBy : 21,
    comments : [{
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
    }]
  }

  constructor() { }

}
