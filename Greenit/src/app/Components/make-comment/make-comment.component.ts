import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommentService } from '../../Services/comment.service';
import { SignInService } from '../../Services/sign-in.service';

@Component({
  selector: 'app-make-comment',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './make-comment.component.html',
  styleUrl: './make-comment.component.css'
})
export class MakeCommentComponent implements OnInit {
  @Input() id! : number;

  @Input() type! : string;

  username : string = '';

  content : string = '';

  constructor(private commentService : CommentService, private authService : SignInService) { }

  ngOnInit() {
    this.authService.getUserSubject().subscribe((username) => {
      this.username = username!.username;
    });
  }


  submitComment() {
    switch (this.type) {
      case 'post':
        this.commentService.commentAPost({postId : this.id, content : this.content, createdBy : this.username}).subscribe(() => {
          window.location.reload();
        });
        break;
      case 'event':
        this.commentService.commentAnEvent({eventId : this.id, content : this.content, createdBy : this.username}).subscribe(() => {
          window.location.reload();
        });
        break;
      case 'comment':
        this.commentService.commentAComment({parentCommentId : this.id, content : this.content, createdBy : this.username}).subscribe(() => {
          window.location.reload();
        });
        break;
    }
  }
}
