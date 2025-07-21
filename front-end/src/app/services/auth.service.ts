import { Injectable } from '@angular/core';
import {LoginUser} from '../models/login-user';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {catchError, map, Observable, tap, throwError} from 'rxjs';
import {ApiResponse} from '../models/api-reponse';
import {RegisterUser} from '../models/register-user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl: string ="http://localhost:5034/api";
  // private tokenKey: string = " ";
  httpClient: HttpClient;

  constructor(_httpClient: HttpClient) {
  this.httpClient = _httpClient;

  }

login(loginData: LoginUser): Observable<any> {
    return this.httpClient.post<any>(`${this.baseUrl}/auth/login`, loginData)
  .pipe(
    catchError(this.handleError)
  )
}
  signUp(signUpData: any): Observable<any> {
    return this.httpClient.post<any>(`${this.baseUrl}/auth/register`, signUpData)
      .pipe(
        catchError(this.handleError)
      );
  }


  private saveToken(token: string): void {
    localStorage.setItem("auth", token);
  }

  // Error handling method
  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = `Client-side error: ${error.error.message}`;
    } else {
      // Server-side error
      if (error.status === 400) {
        errorMessage = 'Invalid email or password.';
      } else if (error.status === 401) {
        errorMessage = 'Unauthorized: Please log in again.';
      } else if (error.status === 500) {
        errorMessage = 'Server error: Please try again later.';
      } else {
        errorMessage = `Server-side error: ${error.status} - ${error.message}`;
      }
    }
    // Return an observable with a user-friendly error message
    console.error('HTTP Error:', error); // Log the full error object
    return throwError(() => new Error(errorMessage));
  }
}
