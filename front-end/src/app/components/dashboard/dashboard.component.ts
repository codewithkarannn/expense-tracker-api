import { Component, inject, input, OnInit, signal } from '@angular/core';
import { PopupAddTransactionFormComponent } from '../popup-add-transaction-form/popup-add-transaction-form.component';
import { TransactionsService } from '../../services/transactions.service';
import { ApiResponse } from '../../models/api-reponse';
import {
  TransactionCategory,
  TransactionType,
  Transaction,
  TransactionSummary,
  TransactionPaymentMode,
  UserDetails,
} from '../../models/login-user';
import { OverviewComponent } from '../overview/overview.component';
import { NgClass } from '@angular/common';
import { AllTransactionsComponent } from '../all-transactions/all-transactions.component';
import { TransactionStateService } from '../../services/transaction-state.service';
import { ToastServiceService } from '../../services/toast-service.service';
import { Router } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTooltipModule } from 'ng-zorro-antd/tooltip';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzTabsComponent, NzTabsModule } from 'ng-zorro-antd/tabs';
import { FormsModule } from '@angular/forms';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-dashboard',

  imports: [
    PopupAddTransactionFormComponent,
    NgClass,
    AllTransactionsComponent,
    NzIconModule,
    NzAvatarModule,
    NzButtonModule,
    NzTooltipModule,
    NzSkeletonModule,
    NzCardModule,
    OverviewComponent,
    NzTabsComponent,
    NzTabsModule,
    FormsModule,
    NzSelectModule,
    NzSkeletonModule,
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  parseFloat = parseFloat;

  transactionTypes = signal<TransactionType[]>([]);
  transactionSummary = <TransactionSummary>{};
  transactionCategories = signal<TransactionCategory[]>([]);
  transactionPaymentMode = signal<TransactionPaymentMode[]>([]);
  userDetails = signal<UserDetails>({} as UserDetails);

  // Using inject to get instances of services
  state = inject(TransactionStateService);
  transactionsService = inject(TransactionsService);
  toast = inject(ToastServiceService);
  router = inject(Router);
  userService = inject(AuthService);

  userIntials = signal<string>('U'); // Default initials
  showAddTransactionForm = signal(false);
  activeTab: string = 'overview'; // Default active tab
  userId = '';
  authToken: string | null = null;
  selectedTabIndex = 0;
  tabNames: string[] = ['overview', 'analytics', 'transaction', 'budget'];
  selectedTab = 'expense'; // Default
  tabOptions = [
    { label: 'Expense', value: 'expense' },
    { label: 'Income', value: 'income' },
  ];

  ngOnInit() {
    this.state.tab$.subscribe((tab) => {
      this.activeTab = tab;
    });

    this.authToken = localStorage.getItem('auth_token');
    if (this.authToken != null) {
      this.userId = this.extractUserIdFromToken(this.authToken);
    }
    this.getTransactionSummary();
    this.getallTransactionTypes();
    this.getallTransactionCategories();
    this.getallTransactionPaymentMode();
    this.getuserDetails();
  }
  tabLabel(tab: string): string {
    return tab.charAt(0).toUpperCase() + tab.slice(1); // 'overview' → 'Overview'
  }

  // Capitalize label for display
  capitalize(tab: string): string {
    return tab.charAt(0).toUpperCase() + tab.slice(1);
  }

  // Update tab state using your existing logic
  onNzTabChange(index: number): void {
    const selectedTab = this.tabNames[index];
    this.setActiveTab(selectedTab); // Your existing logic
  }
  onTabChange(index: number): void {
    const selectedTab = this.tabNames[index];
    this.setActiveTab(selectedTab); // existing function
  }

  getuserInitials(): string {
    const user = this.userDetails();
    if (user && user.firstName && user.lastName) {
      this.userIntials.set(
        user.firstName.charAt(0).toUpperCase() +
          user.lastName.charAt(0).toUpperCase(),
      );
    }
    return 'U';
  }

  logout() {
    localStorage.removeItem('auth_token');
    this.toast.show({
      message: 'Login out successful!',
      type: 'success',
      duration: 3000,
    });
    this.router.navigate(['/login']);
  }

  getallTransactionTypes(): void {
    console.log('Fetching transaction types...');
    console.log('User ID:', this.userId);
    this.transactionsService.getAllTransactionTypes(this.userId).subscribe({
      next: (response: ApiResponse<TransactionType[]>) => {
        if (response.success) {
          this.transactionTypes.set(response.data);
        }
      },
      error: (error) => {},
    });
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
      const payloadBase64 = parts[1].replace(/-/g, '+').replace(/_/g, '/');
      const payloadJson = decodeURIComponent(
        atob(payloadBase64)
          .split('')
          .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join(''),
      );

      const payload = JSON.parse(payloadJson);

      // Debug: Log the entire payload to verify structure
      console.log('Full token payload:', payload);

      // Try different common claim names for user ID
      const userId =
        payload[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
        ] ||
        payload.nameidentifier ||
        payload.sub ||
        payload.userId ||
        '';

      if (!userId) {
        console.warn(
          'No user ID found in token. Available claims:',
          Object.keys(payload),
        );
      }

      return userId;
    } catch (e) {
      console.error('Error parsing token:', e);
      return '';
    }
  }
  getTransactionSummary(): void {
    (console.log('Fetching data started...'),
      this.transactionsService.getTransactionSummary(this.userId).subscribe({
        next: (response: ApiResponse<TransactionSummary>) => {
          if (response.success) {
            this.transactionSummary = response.data;
            console.log('Fetching data started...');
          }
        },
        error: (error) => {},
      }));
  }

  getuserDetails(): void {
    this.userService.getUserDetails(this.userId).subscribe({
      next: (response: ApiResponse<UserDetails>) => {
        this.userDetails.set(response.data);
        this.getuserInitials();
      },
      error: (error) => {
        console.error('Error fetching user details:', error);
      },
    });
  }
  onCategoryAdded(category: TransactionCategory) {
    this.transactionCategories.update((currentCategories) => {
      // The update function must return the new value for the signal
      return [...currentCategories, category];
    });
    // Optionally, you can also show a success message or perform other actions
    this.toast.show({
      message: 'Category added successfully!',
      type: 'success',
      duration: 3000,
    });
  }

  onTypeAdded(type: TransactionType) {
    type.isCustom = true;
    // Add the new category to the transactionCategories array
    this.transactionTypes.update((currentTypes) => {
      // The update function must return the new value for the signal
      return [...currentTypes, type];
    });
    // Optionally, you can also show a success message or perform other actions
    this.toast.show({
      message: 'Category added successfully!',
      type: 'success',
      duration: 3000,
    });
  }

  onPaymentModeAdded(paymentMode: TransactionPaymentMode) {
    paymentMode.isCustom = true;
    // Add the new category to the transactionCategories array
    this.transactionPaymentMode.update((currentTypes) => {
      // The update function must return the new value for the signal
      return [...currentTypes, paymentMode];
    });
    // Optionally, you can also show a success message or perform other actions
    this.toast.show({
      message: 'Category added successfully!',
      type: 'success',
      duration: 3000,
    });
  }

  getallTransactionCategories(): void {
    this.transactionsService.getAllCategories(this.userId).subscribe({
      next: (response: ApiResponse<TransactionCategory[]>) => {
        if (response.success) {
          this.transactionCategories.set(response.data);
          console.log('transaction:', this.transactionCategories);
        }
      },
      error: (error) => {},
    });
  }

  getallTransactionPaymentMode(): void {
    this.transactionsService
      .getAllTransactionPaymentModes(this.userId)
      .subscribe({
        next: (response: ApiResponse<TransactionPaymentMode[]>) => {
          if (response.success) {
            this.transactionPaymentMode.set(response.data);
            console.log(
              'transaction payment mode:',
              this.transactionPaymentMode(),
            );
          }
        },
        error: (error) => {},
      });
  }

  onTransactionAdded() {
    this.getTransactionSummary();
  }

  onCategoryRemoved(categoryId: number): void {
    this.transactionCategories.update((currentCategories) =>
      currentCategories.filter(
        (category) => category.transactionCategoryMasterId !== categoryId,
      ),
    );
  }

  onTypeRemoved(typeId: number): void {
    // Remove the type from the transactionCategories array
    this.transactionTypes.update((currentTypes) =>
      currentTypes.filter((type) => type.transactionTypeMasterId !== typeId),
    );
  }

  onPaymentModeRemoved(paymentModeId: number): void {
    this.transactionPaymentMode.update((currentTypes) =>
      currentTypes.filter((type) => type.paymentModeId !== paymentModeId),
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

  navigateToAllTransactions() {
    this.setActiveTab('transaction');
  }

  openPopup() {
    this.showAddTransactionForm.set(true);
  }

  onClosePopup() {
    this.showAddTransactionForm.set(false);
  }
}
