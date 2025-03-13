import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, throwError } from 'rxjs';
import { TokenService } from './token.service';
import { TokenInfo } from '../Models/token-info';

@Injectable({
  providedIn: 'root'
})
export class SignInService {
  /** API endpoint for authentication-related operations */
  private endpoint: string = "http://localhost:5000/api/Greenitor";

  /** Holds the authenticated user's information */
  private userSubject = new BehaviorSubject<TokenInfo | null>(null);

  /**
   * Constructor initializes the authentication service
   * @param service - TokenService for managing authentication tokens
   * @param client - HttpClient for making HTTP requests
   */
  constructor(private service: TokenService, private client: HttpClient) {
    if (this.service.hasToken('user')) {
      this.userSubject.next(JSON.parse(this.service.getToken('user')));
    }
  }

  /**
   * Returns an observable of the authenticated user
   * @returns Observable containing user authentication data
   */
  getUserSubject(): Observable<TokenInfo | null> {
    return this.userSubject.asObservable();
  }

  /**
   * Authenticates a user with email and password
   * @param email - The user's email
   * @param password - The user's password
   * @returns Observable containing the authentication token and user details
   */
  signIn(email: string, password: string): Observable<TokenInfo | any> {
    return this.client.post<TokenInfo | any>(`${this.endpoint}/login`, { email, password })
      .pipe(
        map(user => {
          this.userSubject.next(user);
          this.service.saveToken('user', JSON.stringify(user));
        }),
        catchError(this.handleError<TokenInfo>("signIn"))
      );
  }

  /**
   * Handles errors during HTTP requests
   * @param operation - Name of the operation that failed
   * @param result - Optional default result to return in case of failure
   * @returns Observable throwing an error message
   */
  private handleError<T>(operation = "operation", result?: T) {
    return (error: any): Observable<T> => {
      this.signOut();
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return throwError(() => new Error(error.message));
    };
  }

  /**
   * Checks if a user authentication token exists
   * @returns Boolean indicating whether the user is authenticated
   */
  hasToken(): boolean {
    return this.service.hasToken('user');
  }

  /**
   * Logs out the current user and removes their authentication token
   */
  signOut(): void {
    this.service.deleteToken('user');
    this.userSubject.next(null);
  }

  /**
   * Retrieves notifications for the authenticated user
   * @returns Observable containing the user's notifications
   */
  getUserNotifications(): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/notifications/${this.userSubject.value?.username}`);
  }
}