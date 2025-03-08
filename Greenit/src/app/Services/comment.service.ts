import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Comment } from '../Models/comment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  private endpoint = 'http://localhost:5000/api/comments'

  constructor(private client : HttpClient) { }

  commentAComment(comment : any) : Observable<any> {
    return this.client.post<any>(`${this.endpoint}`, comment);
  }

  commentAPost(comment : any) {
    return this.client.post<any>(`${this.endpoint}/post/comment`, comment);
  }

  commentAnEvent(comment : any) {
    return this.client.post<any>(`${this.endpoint}/event/comment`, comment);
  }

  likeAComment(commentId : number, username : string) : Observable<any> {
    const like = {commentId : commentId, user : username}
    return this.client.post<any>(`${this.endpoint}/comment/like`, like);
  }

  unlikeAComment(commentId : number, username : string) : Observable<any> {
    const unlike = {commentId : commentId, user : username}
    return this.client.delete<any>(`${this.endpoint}/comment/like`, {body : unlike});
  }

  getCommentLikeCount(commentId : number) : Observable<any> {
    return this.client.get<any>(`${this.endpoint}/likesCount/${commentId}`);
  }

  getCommentLikeByUsername(commentId : number, username : string) : Observable<any> {
    return this.client.get<any>(`${this.endpoint}/${commentId}/likes/${username}`);
  }

  getCommentsFromComment(commentId : number) : Observable<any> {
    return this.client.get<any>(`${this.endpoint}/${commentId}/comments`);
  }
}
