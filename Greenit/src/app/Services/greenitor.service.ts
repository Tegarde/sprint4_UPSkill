import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GreenitorService {
  private endpoint = 'http://localhost:5000/api/greenitor';

  constructor(private client : HttpClient) { }

  getGreenitorStats(username : string) : Observable<any> {
      return this.client.get<any>(`${this.endpoint}/stats/${username}`);
    }

  getGreenitorNotifications(username : string) : Observable<any> {
    return this.client.get<any>(`${this.endpoint}/notifications/${username}`);
  }
  
}
