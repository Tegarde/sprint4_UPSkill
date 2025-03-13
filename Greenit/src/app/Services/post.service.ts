import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Post } from '../Models/post';
import { UpdatePost } from '../Models/update-post';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  private endpoint = 'http://localhost:5000/api/posts';

  constructor(private client : HttpClient) { }

  getAllPosts() : Observable<any[]> {
    return this.client.get<any[]>(this.endpoint);
  }

  resetPostInteractions(postId : number) : Observable<any> {
    return this.client.patch<any>(`${this.endpoint}/reset/${postId}`, {});
  }

  getTrendingPosts() : Observable<Post[]> {
    return this.client.get<Post[]>(`${this.endpoint}/top/5`);
  }

  getPostById(id : number) : Observable<Post> {
    return this.client.get<Post>(`${this.endpoint}/${id}`);
  }

  getPostInteractions(id : number) : Observable<any> {
    return this.client.get<any>(`${this.endpoint}/likesAndDislikes/${id}`);
  }

  getInteractionByUser(id : number, username : string) : Observable<any> {
    return this.client.get<any>(`${this.endpoint}/${id}/interactions/${username}`);
  }

  likeAPost(id : number, username : string) : Observable<any> {
    const like = {postId : id, user : username};
    return this.client.post<any>(`${this.endpoint}/like`, like);
  }

  dislikeAPost(id : number, username : string) : Observable<any> {
    const dislike = {postId : id, user : username};
    return this.client.post<any>(`${this.endpoint}/dislike`, dislike);
  }

  unlikeAPost(id : number, username : string) : Observable<any> {
    const unlike = {postId : id, user : username};
    return this.client.delete<any>(`${this.endpoint}/like`, {body : unlike});
  }

  undislikeAPost(id : number, username : string) : Observable<any> {
    const undislike = {postId : id, user : username};
    return this.client.delete<any>(`${this.endpoint}/dislike`, {body : undislike});
  }

  searchPosts(query : string) : Observable<Post[]> {
    return this.client.get<Post[]>(`${this.endpoint}/search/${query}`);
  }

  createPost(post : any) : Observable<any> {
    return this.client.post<any>(this.endpoint, post);
  }

  getMonthlyTrendingPosts(topN : number) : Observable<any> {
    return this.client.get<any>(`${this.endpoint}/monthly/${topN}`);
  }

  getHottestPosts(topN : number) : Observable<any> {
    return this.client.get<any>(`${this.endpoint}/hottest/${topN}`);
  }

  getDailyTrendingPosts(topN : number) : Observable<any> {
    return this.client.get<any>(`${this.endpoint}/daily/${topN}`);
  }

  addFavoritePost(id : number, username : string) : Observable<any> {
    return this.client.post<any>(`${this.endpoint}/favorite`, {postId : id, user : username});
  }

  removeFavoritePost(id : number, username : string) : Observable<any> {
    return this.client.delete<any>(`${this.endpoint}/favorite`, {body : {postId : id, user : username}});
  }

  isPostFavorite(id : number, username : string) : Observable<any> {
    return this.client.get<any>(`${this.endpoint}/${id}/favorite/${username}`);
  }
  
  getPostsByUser(username : string) : Observable<Post[]> {
    return this.client.get<Post[]>(`${this.endpoint}/user/${username}`);
  }

  getFavoritePosts(username : string) : Observable<Post[]> {
    return this.client.get<Post[]>(`${this.endpoint}/favoritePosts/${username}`);
  }

  changePostStatus(id: number, status: boolean) : Observable<any> {
    return this.client.put<any>(`${this.endpoint}/${id}/status/${status}`, {});
  }

  updatePost(postId: number, updatedPost: UpdatePost): Observable<any> {
    return this.client.put<any>(`${this.endpoint}/${postId}`, updatedPost);
  }

}
