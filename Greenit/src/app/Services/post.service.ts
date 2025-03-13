import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Post } from '../Models/post';
import { UpdatePost } from '../Models/update-post';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  /** API endpoint for post-related operations */
  private endpoint = 'http://localhost:5000/api/posts';

  /**
   * Constructor initializes the HTTP client
   * @param client - HttpClient for making HTTP requests
   */
  constructor(private client: HttpClient) {}

  /**
   * Retrieves all posts
   * @returns Observable containing an array of all posts
   */
  getAllPosts(): Observable<any[]> {
    return this.client.get<any[]>(this.endpoint);
  }

  /**
   * Resets interactions (likes and dislikes) for a specific post
   * @param postId - The ID of the post to reset interactions
   * @returns Observable containing the API response
   */
  resetPostInteractions(postId: number): Observable<any> {
    return this.client.patch<any>(`${this.endpoint}/reset/${postId}`, {});
  }

  /**
   * Retrieves the top 5 trending posts
   * @returns Observable containing an array of trending posts
   */
  getTrendingPosts(): Observable<Post[]> {
    return this.client.get<Post[]>(`${this.endpoint}/top/5`);
  }

  /**
   * Retrieves a specific post by ID
   * @param id - The ID of the post
   * @returns Observable containing the post details
   */
  getPostById(id: number): Observable<Post> {
    return this.client.get<Post>(`${this.endpoint}/${id}`);
  }

  /**
   * Retrieves the number of likes and dislikes for a post
   * @param id - The ID of the post
   * @returns Observable containing interaction statistics
   */
  getPostInteractions(id: number): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/likesAndDislikes/${id}`);
  }

  /**
   * Retrieves a user's interaction with a specific post
   * @param id - The ID of the post
   * @param username - The username of the user
   * @returns Observable containing the interaction details
   */
  getInteractionByUser(id: number, username: string): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/${id}/interactions/${username}`);
  }

  /**
   * Likes a post
   * @param id - The ID of the post
   * @param username - The username of the user liking the post
   * @returns Observable containing the API response
   */
  likeAPost(id : number, username : string) : Observable<any> {
    const like = {postId : id, user : username};
    return this.client.post<any>(`${this.endpoint}/like`, like);
  }

  /**
   * Dislikes a post
   * @param id - The ID of the post
   * @param username - The username of the user disliking the post
   * @returns Observable containing the API response
   */
  dislikeAPost(id : number, username : string) : Observable<any> {
    const dislike = {postId : id, user : username};
    return this.client.post<any>(`${this.endpoint}/dislike`, dislike);
  }

  /**
   * Removes a like from a post
   * @param id - The ID of the post
   * @param username - The username of the user unliking the post
   * @returns Observable containing the API response
   */
  unlikeAPost(id : number, username : string) : Observable<any> {
    const unlike = {postId : id, user : username};
    return this.client.delete<any>(`${this.endpoint}/like`, {body : unlike});
  }

  /**
   * Removes a dislike from a post
   * @param id - The ID of the post
   * @param username - The username of the user undisliking the post
   * @returns Observable containing the API response
   */
  undislikeAPost(id : number, username : string) : Observable<any> {
    const undislike = {postId : id, user : username};
    return this.client.delete<any>(`${this.endpoint}/dislike`, {body : undislike});
  }

  /**
   * Searches posts based on a query
   * @param query - The search query
   * @returns Observable containing an array of matching posts
   */
  searchPosts(query : string): Observable<Post[]> {
    return this.client.get<Post[]>(`${this.endpoint}/search/${query}`);
  }

  /**
   * Creates a new post
   * @param post - The post data
   * @returns Observable containing the API response
   */
  createPost(post: any): Observable<any> {
    return this.client.post<any>(this.endpoint, post);
  }

  /**
   * Retrieves the top N trending posts of the month
   * @param topN - The number of top posts to retrieve
   * @returns Observable containing an array of top monthly posts
   */
  getMonthlyTrendingPosts(topN: number): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/monthly/${topN}`);
  }

  /**
   * Retrieves the top N hottest posts
   * @param topN - The number of hottest posts to retrieve
   * @returns Observable containing an array of hottest posts
   */
  getHottestPosts(topN: number): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/hottest/${topN}`);
  }

  /**
   * Retrieves the top N daily trending posts
   * @param topN - The number of top daily posts to retrieve
   * @returns Observable containing an array of top daily posts
   */
  getDailyTrendingPosts(topN: number): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/daily/${topN}`);
  }

  /**
   * Adds a post to the user's favorites
   * @param id - The ID of the post
   * @param username - The username of the user adding to favorites
   * @returns Observable containing the API response
   */
  addFavoritePost(id: number, username: string): Observable<any> {
    return this.client.post<any>(`${this.endpoint}/favorite`, { postId: id, user: username });
  }

  /**
   * Removes a post from the user's favorites
   * @param id - The ID of the post
   * @param username - The username of the user removing from favorites
   * @returns Observable containing the API response
   */
  removeFavoritePost(id: number, username: string): Observable<any> {
    return this.client.delete<any>(`${this.endpoint}/favorite`, { body: { postId: id, user: username } });
  }

  /**
   * Checks if a post is favorited by the user
   * @param id - The ID of the post
   * @param username - The username of the user
   * @returns Observable indicating whether the post is favorited
   */
  isPostFavorite(id: number, username: string): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/${id}/favorite/${username}`);
  }

  /**
   * Retrieves all posts created by a specific user
   * @param username - The username of the user
   * @returns Observable containing an array of user posts
   */
  getPostsByUser(username: string): Observable<Post[]> {
    return this.client.get<Post[]>(`${this.endpoint}/user/${username}`);
  }

  /**
   * Retrieves all favorite posts of a user
   * @param username - The username of the user
   * @returns Observable containing an array of favorite posts
   */
  getFavoritePosts(username: string): Observable<Post[]> {
    return this.client.get<Post[]>(`${this.endpoint}/favoritePosts/${username}`);
  }

  /**
   * Changes the status of a post
   * @param id - The ID of the post
   * @param status - The new status of the post
   * @returns Observable containing the API response
   */
  changePostStatus(id: number, status: boolean): Observable<any> {
    return this.client.put<any>(`${this.endpoint}/${id}/status/${status}`, {});
  }

  /**
   * Updates a post with new data
   * @param postId - The ID of the post to update
   * @param updatedPost - The updated post data
   * @returns Observable containing the API response
   */
  updatePost(postId: number, updatedPost: UpdatePost): Observable<any> {
    return this.client.put<any>(`${this.endpoint}/${postId}`, updatedPost);
  }
}