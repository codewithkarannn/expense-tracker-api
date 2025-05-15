import { Injectable } from '@angular/core';
import {catchError, Observable, throwError} from 'rxjs';

import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {ApiResponse} from '../models/api-reponse';
import { Transaction, TransactionCategory, TransactionSummary, TransactionType } from '../models/login-user';
@Injectable({
  providedIn: 'root'
})
export class TransactionsService {
  private baseUrl: string ="https://localhost:7238/api";
  // private tokenKey: string = " ";
  httpClient: HttpClient;

  constructor(_httpClient: HttpClient) {

    this.httpClient = _httpClient;

  }

  // Updated method with proper typing
  getAllTransactionTypes(): Observable<ApiResponse<TransactionType[]>> {
    return this.httpClient.get<ApiResponse<TransactionType[]>>(
      `${this.baseUrl}/Transaction/transactionTypes`
    ).pipe(
      catchError(error => {
        console.error('Error fetching transaction types:', error);
        throw error; // Or handle it differently
      })
    );
  }

  getTransactionSummary( userid : string): Observable<ApiResponse<TransactionSummary>> {
    return this.httpClient.get<ApiResponse<TransactionSummary>>(
      `${this.baseUrl}/Transaction/transactionsummary?userId=${userid}`
    ).pipe(
      catchError(error => {
        console.error('Error fetching transaction types:', error);
        throw error; // Or handle it differently
      })
    );
  }

  getTransactionCount( userid : string , numberOfMonths : number): Observable<ApiResponse<number>> {
    return this.httpClient.get<ApiResponse<number>>(
      `${this.baseUrl}/Transaction/transactionscount/${userid}/${numberOfMonths}`
    ).pipe(
      catchError(error => {
        
        throw error; // Or handle it differently
      })
    );
  }
  getAllTransactions( userid : string): Observable<ApiResponse<Transaction[]>> {
    return this.httpClient.get<ApiResponse<Transaction[]>>(
      `${this.baseUrl}/Transaction/transactions/${userid}`
    ).pipe(
      catchError(error => {
        console.error('Error fetching transaction types:', error);
        throw error; // Or handle it differently
      })
    );
  }

  getRecentTransaction( userid : string): Observable<ApiResponse<Transaction[]>> {
    return this.httpClient.get<ApiResponse<Transaction[]>>(
      `${this.baseUrl}/Transaction/recent-transactions/${userid}`
    ).pipe(
      catchError(error => {
        console.error('Error fetching transaction types:', error);
        throw error; // Or handle it differently
      })
    );
  }

  getAllCategories(): Observable<ApiResponse<TransactionCategory[]>> {
    return this.httpClient.get<ApiResponse<TransactionCategory[]>>(
      `${this.baseUrl}/Transaction/transactioncategories`
    ).pipe(
      catchError(error => {
        console.error('Error fetching transaction cateogires:', error);
        throw error; // Or handle it differently
      })
    );
  }



  addTransaction(transaction: Transaction): Observable<ApiResponse<Transaction>> {
    return this.httpClient.post<ApiResponse<Transaction>>( `${this.baseUrl}/Transaction/addtransaction`, transaction).pipe(
      catchError(this.handleError)
    );
  }

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
