import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateEvent } from '../Models/create-event';
import { Observable } from 'rxjs';
import { Evento } from '../Models/evento';

@Injectable({
  providedIn: 'root'
})
export class EventService {
 private endpoint = 'http://localhost:5000/api/event'

  constructor(private client : HttpClient) { }

  createEvent(formData: FormData): Observable<any> {
    return this.client.post<any>(`${this.endpoint}`, formData);
  }

  getEventsByStatus(status: string): Observable<Evento[]> {
    return this.client.get<Evento[]>(`${this.endpoint}/status/${status}`);
  }
  
  getEventById(id: number): Observable<Evento> {
    return this.client.get<Evento>(`${this.endpoint}/${id}`);
  }
}
