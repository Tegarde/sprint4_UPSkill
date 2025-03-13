import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  /**
   * Retrieves a token from local storage
   * @param key - The key of the token to retrieve
   * @returns The token value or an empty string if not found
   */
  getToken(key: string): string {
    return localStorage.getItem(key) ?? '';
  }

  /**
   * Checks if a token exists in local storage
   * @param key - The key of the token to check
   * @returns Boolean indicating whether the token exists
   */
  hasToken(key: string): boolean {
    if (typeof localStorage === 'undefined') return false;
    return !!this.getToken(key);
  }

  /**
   * Saves a token to local storage
   * @param key - The key under which the token will be stored
   * @param token - The token value to store
   */
  saveToken(key: string, token: string): void {
    return localStorage.setItem(key, token);
  }

  /**
   * Deletes a token from local storage
   * @param key - The key of the token to remove
   */
  deleteToken(key: string): void {
    localStorage.removeItem(key);
  }

  /**
   * Checks if a JWT token is valid
   * - Ensures the token exists
   * - Decodes and verifies its expiration date
   * @returns Boolean indicating whether the token is valid
   */
  hasValidToken(): boolean {
    const token = this.getToken('jwtToken');
    if (!token) return false;

    try {
      const tokenParts = token.split('.');
      if (tokenParts.length !== 3) return false;

      const tokenPayload = JSON.parse(atob(tokenParts[1]));
      if (!tokenPayload.exp) return false;

      const expiration = tokenPayload.exp * 1000;
      return expiration > Date.now();
    } catch (error) {
      console.error('Erro ao validar o token:', error);
      return false;
    }
  }
}