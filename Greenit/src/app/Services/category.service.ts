import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private endpoint : string = "http://localhost:5000/api/Category"

  constructor(private client : HttpClient) { }

  getCategories() : Observable<any> {
    return this.client.get<any>(this.endpoint);
  }

  createCategory(category : string) : Observable<any> {
    return this.client.post<any>(this.endpoint, {description : category});
  }

  deleteCategory(description : string) : Observable<any> {
    return this.client.delete<any>(`${this.endpoint}/${description}`);    
  }
}
