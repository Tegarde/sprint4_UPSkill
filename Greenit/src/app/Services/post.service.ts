import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Post } from '../Models/post';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  private endpoint = 'http://localhost:5000/api/posts';

  constructor(private client : HttpClient) { }

  getTrendingPosts() : Observable<Post[]> {
    return this.client.get<Post[]>(`${this.endpoint}/top/5`);
  }

  getPostById(id : number) : Observable<Post> {
    return this.client.get<Post>(`${this.endpoint}/${id}`);
  }
}
