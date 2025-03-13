import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GreenitorComplete } from '../Models/greenitor-complete';
import { ResponseMessage } from '../Models/response-message';

@Injectable({
  providedIn: 'root'
})
export class GreenitorService {
  /** API endpoint for Greenitor-related operations */
  private endpoint = 'http://localhost:5000/api/greenitor';

  /**
   * Constructor initializes the HTTP client
   * @param client - HttpClient for making HTTP requests
   */
  constructor(private client: HttpClient) {}

  /**
   * Retrieves statistics for a specific user
   * @param username - The username of the Greenitor
   * @returns Observable containing user statistics
   */
  getGreenitorStats(username: string): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/stats/${username}`);
  }

  /**
   * Retrieves notifications for a specific user
   * @param username - The username of the Greenitor
   * @returns Observable containing user notifications
   */
  getGreenitorNotifications(username: string): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/notifications/${username}`);
  }

  /**
   * Retrieves a list of all Greenitor users
   * @returns Observable containing an array of GreenitorComplete objects
   */
  getAllGreenitors(): Observable<GreenitorComplete[]> {
    return this.client.get<GreenitorComplete[]>(`${this.endpoint}/all`);
  }

  /**
   * Uploads a profile picture for a user
   * @param image - The image file to be uploaded
   * @returns Observable containing the response message with file details
   */
  uploadProfilePicture(image: File): Observable<ResponseMessage> {
    const formData = new FormData();
    formData.append('file', image);
    return this.client.post<ResponseMessage>(`http://localhost:5000/api/FileUpload/upload`, formData);
  }

  /**
   * Retrieves user details by username
   * @param username - The username of the Greenitor
   * @returns Observable containing user data
   */
  getUserByUsername(username: string): Observable<any> {
    return this.client.get<any>(`${this.endpoint}/username/${username}`);
  }

  /**
   * Updates user details
   * @param username - The username of the Greenitor to update
   * @param data - The updated user data
   * @returns Observable containing the API response
   */
  updateUser(username: string, data: any): Observable<any> {
    return this.client.put<any>(`${this.endpoint}/update/${username}`, data);
  }
}