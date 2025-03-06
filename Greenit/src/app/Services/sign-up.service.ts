import { Injectable } from '@angular/core';
import { GreenitorRegister } from '../Models/greenitor-register';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SignUpService {

  constructor(private client : HttpClient) { }

  private endpoint : string = "http://localhost:5000/api/Greenitor";

  register(greenitor : GreenitorRegister) : Observable<any> {
    return this.client.post<any>(this.endpoint, greenitor);
  }

  getUserByUsername(username : string) : Observable<any> {
    return this.client.get<any>(`${this.endpoint}/username/${username}`);
  }
}
