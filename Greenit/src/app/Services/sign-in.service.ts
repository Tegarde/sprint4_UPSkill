import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, throwError } from 'rxjs';
import { TokenService } from './token.service';
import { TokenInfo } from '../Models/token-info';

@Injectable({
  providedIn: 'root'
})
export class SignInService {

  private endpoint : string = "http://localhost:5000/api/Greenitor";
  private userSubject = new BehaviorSubject<TokenInfo | null>(null);


  constructor(private service : TokenService, private client : HttpClient) {
    if (this.service.hasToken('user')) {
      this.userSubject.next(JSON.parse(this.service.getToken('user')));
    }
  }

  getUserSubject() {
    return this.userSubject.asObservable();
  }

  signIn(email:string, password:string) : Observable<TokenInfo | any> {
    return this.client.post<TokenInfo | any>(`${this.endpoint}/login`, {email: email, password: password})
        .pipe(
            map(user => {
              this.userSubject.next(user);
              this.service.saveToken('user', JSON.stringify(user));
            }),
            catchError(this.handleError<TokenInfo>("signIn"))
        )
  }

  handleError<T>(operation = "operation", result?:T) {
    return (error:any) : Observable<T> => {
      this.signOut();
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return throwError(() => new Error(error.message));
    }
  }

  hasToken() {
    return this.service.hasToken('user');
  }

  signOut() {
    this.service.deleteToken('user');
    this.userSubject.next(null);
  }

  getUserNotifications() : Observable<any> {
    return this.client.get<any>(`${this.endpoint}/notifications/${this.userSubject.value?.username}`);    
  }
}
