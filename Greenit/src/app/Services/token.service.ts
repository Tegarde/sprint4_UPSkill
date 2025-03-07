import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor() { }

  getToken(key:string) {
    return localStorage.getItem(key) ?? '';
  }

  hasToken(key:string) {
    if(typeof localStorage == 'undefined') return false;
    return !!this.getToken(key);
  }

  saveToken(key:string, token:string) {
    return localStorage.setItem(key, token);
  }

  deleteToken(key:string) {
    localStorage.removeItem(key);
  }

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
