import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GreenitorComplete } from '../Models/greenitor-complete';
import { ResponseMessage } from '../Models/response-message';

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
  getAllGreenitors() : Observable<GreenitorComplete[]> {
    return this.client.get<GreenitorComplete[]>(`${this.endpoint}/all`);
  }

  uploadProfilePicture(image : File) : Observable<ResponseMessage> {
    const formData = new FormData();
    formData.append('file', image);
    return this.client.post<ResponseMessage>(`http://localhost:5000/api/FileUpload/upload`, formData);
  }

  getUserByUsername(username : string) : Observable<any> {
    return this.client.get<any>(`${this.endpoint}/username/${username}`);
  }

  updateUser(username : string, data : any) : Observable<any> {
    return this.client.put<any>(`${this.endpoint}/update/${username}`, data);
  }
}
