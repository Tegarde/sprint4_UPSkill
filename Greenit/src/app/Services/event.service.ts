import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../Models/evento';
import { AttendEvent } from '../Models/attend-event';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  /** API endpoint for event-related operations */
  private endpoint = 'http://localhost:5000/api/event';

  /**
   * Constructor initializes the HTTP client
   * @param client - HttpClient for making HTTP requests
   */
  constructor(private client: HttpClient) {}

  /**
   * Creates a new event
   * @param formData - Form data containing event details
   * @returns Observable containing the API response
   */
  createEvent(formData: FormData): Observable<any> {
    return this.client.post<any>(`${this.endpoint}`, formData);
  }

  /**
   * Retrieves events filtered by status
   * @param status - The status of the events to retrieve (e.g., 'Open', 'Closed')
   * @returns Observable containing a list of events
   */
  getEventsByStatus(status: string): Observable<Evento[]> {
    return this.client.get<Evento[]>(`${this.endpoint}/status/${status}`);
  }

  /**
   * Retrieves a specific event by ID
   * @param id - The ID of the event to retrieve
   * @returns Observable containing the event details
   */
  getEventById(id: number): Observable<Evento> {
    return this.client.get<Evento>(`${this.endpoint}/${id}`);
  }

  /**
   * Allows a user to attend an event
   * @param attendEvent - The attend event request containing user and event details
   * @returns Observable containing the API response
   */
  attendEvent(attendEvent: AttendEvent): Observable<any> {
    return this.client.post<any>(`${this.endpoint}/attend`, attendEvent);
  }

  /**
   * Checks if a user is attending an event
   * @param eventId - The ID of the event
   * @param username - The username of the user
   * @returns Observable indicating whether the user is attending
   */
  isAttending(eventId: number, username: string): Observable<any> {
    return this.client.get<AttendEvent>(`${this.endpoint}/isAttending/${eventId}/${username}`);
  }

  /**
   * Allows a user to unattend an event
   * @param unattendEvent - The unattend event request containing user and event details
   * @returns Observable containing the API response
   */
  unattendEvent(unattendEvent: AttendEvent): Observable<any> {
    return this.client.delete<any>(`${this.endpoint}/unattend`, { body: unattendEvent });
  }

  /**
   * Updates the status of an event
   * @param eventId - The ID of the event to update
   * @param status - The new status of the event
   * @returns Observable containing a message response
   */
  changeEventStatus(eventId: number, status: string): Observable<{ message: string }> {
    return this.client.put<{ message: string }>(`${this.endpoint}/status/${eventId}`, { status });
  }
}