import { Component, inject, input, OnInit, signal } from '@angular/core';
import { PopupAddTransactionFormComponent } from '../popup-add-transaction-form/popup-add-transaction-form.component';
import { TransactionsService } from '../../services/transactions.service';
import { ApiResponse } from '../../models/api-reponse';
import { TransactionCategory, TransactionType, Transaction, TransactionSummary } from '../../models/login-user';
import { OverviewComponent } from '../overview/overview.component';
import { NgClass } from '@angular/common';
import { AllTransactionsComponent } from '../all-transactions/all-transactions.component';
import { TransactionStateService } from '../../services/transaction-state.service';
import { ToastServiceService } from '../../services/toast-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',

  imports: [
    PopupAddTransactionFormComponent,
    NgClass,
    AllTransactionsComponent,
    OverviewComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'

})

export class DashboardComponent implements OnInit {
  transactionTypes = <TransactionType[]>([]);
  transactionSummary = <TransactionSummary>{};
  transactionCategories = <TransactionCategory[]>([]);
  state = inject(TransactionStateService);
  showAddTransactionForm = signal(false);
  activeTab: string = 'overview'; // Default active tab
  userId = '';
  authToken: string | null = null;

  constructor(private transactionsService: TransactionsService, private toast: ToastServiceService, private router: Router) {
  }
  ngOnInit() {
    this.state.tab$.subscribe(tab => {
      this.activeTab = tab;
    });

    this.authToken = localStorage.getItem('auth_token');
    if (this.authToken != null) {
      this.userId = this.extractUserIdFromToken(this.authToken);
    }
    ;
    this.getTransactionSummary();
    this.getallTransactionTypes();
    this.getallTransactionCategories();
  }
  logout() {
    localStorage.removeItem('auth_token');
    this.toast.show({
      message: 'Login out successful!',
      type: 'success',
      duration: 3000
    });
    this.router.navigate(['/login']);
  }

  getallTransactionTypes(): void {
    console.log('Fetching transaction types...');
    console.log('User ID:', this.userId);
    this.transactionsService.getAllTransactionTypes(this.userId).subscribe(
      {
        next: (response: ApiResponse<TransactionType[]>) => {

          if (response.success) {
            this.transactionTypes = response.data;

          }

        },
        error: (error) => {

        }
      }
    );
  }
  private extractUserIdFromToken(token: string): string {
    try {
      // Remove 'Bearer ' prefix if present
      const actualToken = token.replace(/^Bearer\s+/i, '');

      // Split the token
      const parts = actualToken.split('.');
      if (parts.length !== 3) {
        throw new Error('Invalid token format - expected 3 parts');
      }

      // Base64Url decode with proper padding
      const payloadBase64 = parts[1]
        .replace(/-/g, '+')
        .replace(/_/g, '/');
      const payloadJson = decodeURIComponent(
        atob(payloadBase64)
          .split('')
          .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      );

      const payload = JSON.parse(payloadJson);

      // Debug: Log the entire payload to verify structure
      console.log('Full token payload:', payload);

      // Try different common claim names for user ID
      const userId =
        payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] ||
        payload.nameidentifier ||
        payload.sub ||
        payload.userId ||
        '';

      if (!userId) {
        console.warn('No user ID found in token. Available claims:', Object.keys(payload));
      }

      return userId;
    } catch (e) {
      console.error('Error parsing token:', e);
      return '';
    }
  }
  getTransactionSummary(): void {
    console.log('Fetching data started...'),
      this.transactionsService.getTransactionSummary(this.userId).subscribe(
        {

          next: (response: ApiResponse<TransactionSummary>) => {

            if (response.success) {
              this.transactionSummary = response.data;
              console.log('Fetching data started...')
            }

          },
          error: (error) => {

          }
        }
      );

  }
onCategoryAdded(category: TransactionCategory ){

    // Add the new category to the transactionCategories array
    this.transactionCategories.push(category);
    // Optionally, you can also show a success message or perform other actions
    this.toast.show({
      message: 'Category added successfully!',
      type: 'success',
      duration: 3000
    });
}

onTypeAdded(type: TransactionType ){

    // Add the new category to the transactionCategories array
    this.transactionTypes.push(type);
    // Optionally, you can also show a success message or perform other actions
    this.toast.show({
      message: 'Category added successfully!',
      type: 'success',
      duration: 3000
    });
}

getallTransactionCategories(): void {
    this.transactionsService.getAllCategories(this.userId).subscribe(
      {
        next: (response: ApiResponse<TransactionCategory[]>) => {

          if (response.success) {
            this.transactionCategories = response.data;
            console.log("transaction:", this.transactionCategories);
          }

        },
        error: (error) => {

        }
      }
    );
  }



  // Function to change tabs
  setActiveTab(tab: string): void {

    this.state.setActiveTab(tab);

  }

  // Helper function to check if a tab is active
  isTabActive(tab: string): boolean {
    return this.activeTab === tab;
  }



  openPopup() {
    this.showAddTransactionForm.set(true);

  }

  onClosePopup() {
    this.showAddTransactionForm.set(false);


  }
}
