import { Injectable } from '@angular/core';
import {catchError, Observable, throwError} from 'rxjs';

import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {ApiResponse, PaginatedResponse, TransactionRequestParams} from '../models/api-reponse';
import { AddTransactionCategoryMasterDTO, AddTransactionTypeMasterDTO, Transaction, TransactionCategory, TransactionPaymentMode, TransactionSummary, TransactionType } from '../models/login-user';
@Injectable({
  providedIn: 'root'
})
export class TransactionsService {
  private baseUrl: string ="http://localhost:5034/api";
  // private tokenKey: string = " ";
  httpClient: HttpClient;

  constructor(_httpClient: HttpClient) {

    this.httpClient = _httpClient;

  }




   addTransactionCategory(category: AddTransactionCategoryMasterDTO): Observable<ApiResponse<TransactionCategory>> {
    return this.httpClient.post<ApiResponse<TransactionCategory>>(`${this.baseUrl}/Transaction/addtransactioncategory`, category).pipe(
      catchError(error => {
        console.error('Error fetching transaction types:', error);
        throw error; // Or handle it differently
      })
    );
  }


  addTransactionPayementMode(paymentMode: TransactionPaymentMode): Observable<ApiResponse<TransactionPaymentMode>> {
    return this.httpClient.post<ApiResponse<TransactionPaymentMode>>(`${this.baseUrl}/Transaction/addtransactionpaymentmode`, paymentMode).pipe(
      catchError(error => {
        console.error('Error fetching transaction types:', error);
        throw error; // Or handle it differently
      })
    );
  }
   addTransactionType(transactionType: AddTransactionTypeMasterDTO): Observable<ApiResponse<TransactionType>> {
    return this.httpClient.post<ApiResponse<TransactionType>>(`${this.baseUrl}/Transaction/addtransactiontype`, transactionType).pipe(
      catchError(error => {
        console.error('Error fetching transaction types:', error);
        throw error; // Or handle it differently
      })
    );
  }

  deleteTransactionType(transactionTypeId: number): Observable<ApiResponse<unknown>> {
    return this.httpClient.delete<ApiResponse<any>>(
      `${this.baseUrl}/Transaction/deletetransactiontype/?transactionTypeMasterId=${transactionTypeId}`
    ).pipe(
      catchError(error => {
        console.error('Error deleting transaction type:', error);
        throw error; // Or handle it differently
      })
    );
  }

  deleteTransaction(transactionMasterId: string): Observable<ApiResponse<unknown>> {
    return this.httpClient.delete<ApiResponse<any>>(
      `${this.baseUrl}/Transaction/deletetransaction/${transactionMasterId}`
    ).pipe(
      catchError(error => {
        console.error('Error deleting transaction type:', error);
        throw error; // Or handle it differently
      })
    );
  }

  deleteTransactionCategory(categoryId: number): Observable<ApiResponse<unknown>> {
    return this.httpClient.delete<ApiResponse<any>>(
      `${this.baseUrl}/Transaction/deletetransactioncategory/?transactionCategoryMasterId=${categoryId}`
    ).pipe(
      catchError(error => {
        console.error('Error deleting transaction category:', error);
        throw error; // Or handle it differently
      })
    );
  }

    deleteTransactionPaymentMode(paymentModeId: number): Observable<ApiResponse<unknown>> {
    return this.httpClient.delete<ApiResponse<any>>(
      `${this.baseUrl}/Transaction/deletetransactionpaymentmode/?transactionCategoryMasterId=${paymentModeId}`
    ).pipe(
      catchError(error => {
        console.error('Error deleting transaction payment mode:', error);
        throw error; // Or handle it differently
      })
    );
  }
  // Updated method with proper typing
  getAllTransactionTypes(userId : string): Observable<ApiResponse<TransactionType[]>> {
    return this.httpClient.get<ApiResponse<TransactionType[]>>(
      `${this.baseUrl}/Transaction/transactionTypes?userMasterID=${userId}`
    ).pipe(
      catchError(error => {
        console.error('Error fetching transaction types:', error);
        throw error; // Or handle it differently
      })
    );
  }

getAllTransactionPaymentModes(userId : string): Observable<ApiResponse<TransactionPaymentMode[]>> {
    return this.httpClient.get<ApiResponse<TransactionPaymentMode[]>>(
      `${this.baseUrl}/Transaction/transactionPaymentModes?userMasterID=${userId}`
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

  getAllCategories(userid : string): Observable<ApiResponse<TransactionCategory[]>> {
    return this.httpClient.get<ApiResponse<TransactionCategory[]>>(
      `${this.baseUrl}/Transaction/transactioncategories?userMasterID=${userid}`
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

  getAllPaginatedTransactions(userId: string, params: TransactionRequestParams): Observable<PaginatedResponse<Transaction>> {
    
    // Create a clean object, removing any null, undefined, or empty string values
    // as HttpClient might not ignore them all by default.
    const cleanParams: { [param: string]: any } = {};
    for (const key in params) {
        // This is a type-safe way to access the key
        const value = params[key as keyof TransactionRequestParams];
        if (value !== null && value !== undefined && value !== '') {
            cleanParams[key] = value;
        }
    }

    // Pass the clean object directly to the `params` option.
    // HttpClient will automatically convert it to a query string like:
    // ?page=1&pageSize=10&sortColumn=transactionDate&...
    return this.httpClient.get<PaginatedResponse<Transaction>>(
      `${this.baseUrl}/Transaction/paginatedtransactions/${userId}`, 
      { params: cleanParams }
    );
  }
}
