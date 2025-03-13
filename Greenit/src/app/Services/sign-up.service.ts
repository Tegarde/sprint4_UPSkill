import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SignUpService {
  /** API endpoint for user registration and retrieval */
  private endpoint: string = "http://localhost:5000/api/Greenitor";

  /**
   * Constructor initializes the HTTP client
   * @param client - HttpClient for making HTTP requests
   */
  constructor(private client: HttpClient) {}

  /**
   * Registers a new user
   * @param greenitor - The user registration data
   * @returns Observable containing the API response
   */
  register(greenitor: any): Observable<any> {
    return this.client.post<any>(this.endpoint, greenitor);
  }

  /**
   * Retrieves user details by username
   * @param username - The username of the user
   * @returns Observable containing user data
   */
  getUserByUsername(username: string): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/username/${username}`);
  }
}