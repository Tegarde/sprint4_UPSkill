import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  /** API endpoint for comment-related requests */
  private endpoint = 'http://localhost:5000/api/comments';

  /**
   * Constructor initializes the HTTP client
   * @param client - HttpClient for making HTTP requests
   */
  constructor(private client: HttpClient) { }

  /**
   * Posts a comment on another comment
   * @param comment - The comment data to be posted
   * @returns Observable containing the API response
   */
  commentAComment(comment: any): Observable<any> {
    return this.client.post<any>(`${this.endpoint}`, comment);
  }

  /**
   * Posts a comment on a post
   * @param comment - The comment data to be posted
   * @returns Observable containing the API response
   */
  commentAPost(comment: any): Observable<any> {
    return this.client.post<any>(`${this.endpoint}/post/comment`, comment);
  }

  /**
   * Posts a comment on an event
   * @param comment - The comment data to be posted
   * @returns Observable containing the API response
   */
  commentAnEvent(comment: any): Observable<any> {
    return this.client.post<any>(`${this.endpoint}/event/comment`, comment);
  }

  /**
   * Likes a comment
   * @param commentId - The ID of the comment to like
   * @param username - The username of the user liking the comment
   * @returns Observable containing the API response
   */
  likeAComment(commentId: number, username: string): Observable<any> {
    const like = { commentId: commentId, user: username };
    return this.client.post<any>(`${this.endpoint}/comment/like`, like);
  }

  /**
   * Removes a like from a comment
   * @param commentId - The ID of the comment to unlike
   * @param username - The username of the user unliking the comment
   * @returns Observable containing the API response
   */
  unlikeAComment(commentId: number, username: string): Observable<any> {
    const unlike = { commentId: commentId, user: username };
    return this.client.delete<any>(`${this.endpoint}/comment/like`, { body: unlike });
  }

  /**
   * Retrieves the number of likes for a specific comment
   * @param commentId - The ID of the comment
   * @returns Observable containing the number of likes
   */
  getCommentLikeCount(commentId: number): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/likesCount/${commentId}`);
  }

  /**
   * Checks if a specific user has liked a comment
   * @param commentId - The ID of the comment
   * @param username - The username to check
   * @returns Observable containing the like status
   */
  getCommentLikeByUsername(commentId: number, username: string): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/${commentId}/likes/${username}`);
  }

  /**
   * Retrieves all replies to a specific comment
   * @param commentId - The ID of the parent comment
   * @returns Observable containing the list of replies
   */
  getCommentsFromComment(commentId: number): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/${commentId}/comments`);
  }
}