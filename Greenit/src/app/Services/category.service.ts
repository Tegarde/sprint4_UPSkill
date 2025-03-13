import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  /** API endpoint for category-related requests */
  private endpoint: string = "http://localhost:5000/api/Category";

  /**
   * Constructor initializes the HTTP client
   * @param client - HttpClient for making HTTP requests
   */
  constructor(private client: HttpClient) { }

  /**
   * Retrieves all categories from the API
   * @returns Observable containing the list of categories
   */
  getCategories(): Observable<any> {
    return this.client.get<any>(this.endpoint);
  }

  /**
   * Creates a new category
   * @param category - The name/description of the category to be created
   * @returns Observable containing the API response
   */
  createCategory(category: string): Observable<any> {
    return this.client.post<any>(this.endpoint, { description: category });
  }

  /**
   * Deletes a category by its description
   * @param description - The description of the category to be deleted
   * @returns Observable containing the API response
   */
  deleteCategory(description: string): Observable<any> {
    return this.client.delete<any>(`${this.endpoint}/${description}`);
  }
}