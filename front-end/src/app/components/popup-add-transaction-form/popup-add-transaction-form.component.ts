import { Component, computed, inject, input, OnInit, output, signal } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { TransactionsService } from '../../services/transactions.service';
import { Observable } from 'rxjs/internal/Observable';
import { catchError, lastValueFrom, of } from 'rxjs';
import { AddTransactionCategoryMasterDTO, AddTransactionTypeMasterDTO, Transaction, TransactionCategory, TransactionType } from '../../models/login-user';
import { ApiResponse } from '../../models/api-reponse';
import { TransactionStateService } from '../../services/transaction-state.service';
import { Toast, ToastServiceService } from '../../services/toast-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-popup-add-transaction-form',
  imports: [
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './popup-add-transaction-form.component.html',
  styleUrl: './popup-add-transaction-form.component.css'
})

export class PopupAddTransactionFormComponent implements OnInit {

  transacctionState =  inject(TransactionStateService);
  isVisible = input<boolean>(false);
  
  isSubmitting = false;
  transactionTypes = input<TransactionType[]>([]);
  transactionCategories = input<TransactionCategory[]>([]);
  typeAdded = output<TransactionType>();
  categoryAdded = output<TransactionCategory>();
  typeRemoved = output<number>();
  categoryRemoved = output<number>();
  title = "Add Transaction";
  closePopup = output<void>();
  formSubmit = output<any>();
  dropdownOpen = signal(false);
  categoryDropdownOpen = signal(false);
  typeSearchTerm = signal('');
     // For Category Dropdown
  categorySearchTerm = signal('');

  transaction: Transaction = {
    transactionMasterId: null,
    transactionAmount: 0,
    transactionCategory: "",
    transactionCategoryMasterId: 0,
    transactionDate: new Date().toISOString().split('T')[0],
    transactionDescription: "",
    transactionNote: "",
    transactionType: "",
    transactionTypeMasterId: 0,
    userId:  '',
    createdAt: "",
    isActive: 0,
    deletedAt: null
  };

  constructor( private transactionService: TransactionsService , private toast :  ToastServiceService , private router: Router) {
   
  }

  ngOnInit() {
    this.initializeTransaction();

  }

  filteredTransactionTypes = computed(() => {
    const term = this.typeSearchTerm().toLowerCase();
    if (!term) {
      return this.transactionTypes();
    }
    return this.transactionTypes().filter(type =>
      type.transactionTypename.toLowerCase().includes(term)
    );
  });

  filteredTransactionCategories = computed(() => {
    const term = this.categorySearchTerm().toLowerCase();
    if (!term) {
      return this.transactionCategories();
    }
    return this.transactionCategories().filter(category =>
      category.transactionCategoryName.toLowerCase().includes(term)
    );
  });


   
  async addNewType() {
    console.log('Adding new type:', this.typeSearchTerm());
    if (!this.typeSearchTerm().trim()) return;
    const newTypeDTO  : AddTransactionTypeMasterDTO =  { transactionTypename: this.typeSearchTerm(), userMasterId: this.transaction.userId , transactionTypeMasterId: null  };
    try {
      const response = await lastValueFrom(this.transactionService.addTransactionType(newTypeDTO));
      const newTypeFromApi = response.data;
      console.log('New type added:', newTypeFromApi);

      // CHANGE: Instead of updating a local signal, emit the new type to the parent.
      this.typeAdded.emit(newTypeFromApi);
      
      // Now select the new item. The parent will update the list, and it will flow back down.
      this.selectTransactionType(newTypeFromApi.transactionTypeMasterId);
    } catch (error) {
      console.error('Failed to add transaction type', error);
    }
  }

    
  async addNewCategory() {
    console.log('Adding new category:', this.categorySearchTerm());
    if (!this.categorySearchTerm().trim()) return;
    const newTypeDTO  : AddTransactionCategoryMasterDTO =  { transactionCategoryName: this.categorySearchTerm(), userMasterId: this.transaction.userId , transactionCategoryMasterId: null };
    try {
      const response = await lastValueFrom(this.transactionService.addTransactionCategory(newTypeDTO));
      const newTypeFromApi = response.data;
      
      console.log('New type added:', newTypeFromApi);
      // CHANGE: Instead of updating a local signal, emit the new type to the parent.
      this.categoryAdded.emit(newTypeFromApi);
      
      // Now select the new item. The parent will update the list, and it will flow back down.
      this.selectCategory(newTypeFromApi.transactionCategoryMasterId);
    } catch (error) {
      console.error('Failed to add transaction type', error);
    }
  }



  toggleDropdown() {
    this.dropdownOpen.update(open => !open);
    this.categoryDropdownOpen.set(false);
  }

  toggleCategoryDropdown() {
    this.categoryDropdownOpen.update(open => !open);
    this.dropdownOpen.set(false);
  }

  getSelectedTransactionType(): string {
    const selectedId = this.transaction.transactionTypeMasterId;
    const type = this.transactionTypes().find(t => t.transactionTypeMasterId === selectedId);
    return type ? type.transactionTypename : '';
  }

  getSelectedCategory(): string {
    const selectedId = this.transaction.transactionCategoryMasterId;
    const category = this.transactionCategories().find(c => c.transactionCategoryMasterId === selectedId);
    return category ? category.transactionCategoryName : '';
  }

  selectTransactionType(typeId: number) {
    this.transaction.transactionTypeMasterId = typeId;
    this.transaction.transactionCategoryMasterId = 0;
    const type = this.transactionTypes().find(t => t.transactionTypeMasterId === typeId);
    if (type) {
      this.transaction.transactionType = type.transactionTypename;
    }
    this.dropdownOpen.set(false);
  }

  selectCategory(categoryId: number) {
    this.transaction.transactionCategoryMasterId = categoryId;
    const category = this.transactionCategories().find(c => c.transactionCategoryMasterId === categoryId);
    if (category) {
      this.transaction.transactionCategory = category.transactionCategoryName;
    }
    this.categoryDropdownOpen.set(false);
  }

   removeType(typeId: number) {
    console.log('Removing type with ID:', typeId);
     this.transactionService.deleteTransactionType(typeId).subscribe(
      {
        next: (response: ApiResponse<any>) => {

          if (response.success) {
            // Optionally, you can also show a success message or perform other actions
            this.toast.show({
              message: 'Type removed successfully!',
              type: 'success',
              duration: 3000
            });

            this.typeRemoved.emit(typeId);
          }

        },
        error: (error) => {
          console.error('Error removing type:', error);
          this.toast.show({
            message: 'Error removing type',
            type: 'error',
            duration: 3000
          });
        }
      }
    );


    

    this.typeRemoved.emit(typeId);
  }

   removeCategory(categoryId: number) {
 
    this.transactionService.deleteTransactionCategory(categoryId).subscribe(
      {
        next: (response: ApiResponse<any>) => {

          if (response.success) {
            // Optionally, you can also show a success message or perform other actions
            this.toast.show({
              message: 'Category removed successfully!',
              type: 'success',
              duration: 3000
            });

            this.categoryRemoved.emit(categoryId);
          }

        },
        error: (error) => {
          console.error('Error removing category:', error);
          this.toast.show({
            message: 'Error removing category',
            type: 'error',
            duration: 3000
          });

        }
      }
    );


    this.categoryRemoved.emit(categoryId);
  }

  initializeTransaction(): void {
    const authToken = localStorage.getItem('auth_token');
    
    var userId = '';
    if(authToken != null)
    {
      userId = this.extractUserIdFromToken(authToken);
    }
    
    this.transaction = {
      transactionMasterId: null,
      transactionAmount: 0,
      transactionCategory: "",
      transactionCategoryMasterId: 0,
      transactionDate: new Date().toISOString().split('T')[0],
      transactionDescription: "",
      transactionNote: "",
      transactionType: "",
      transactionTypeMasterId: 0,
      userId: userId,
      createdAt: new Date().toISOString(),
      isActive: 1,
      deletedAt: null
    };
  
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
  onSubmit() {
    this.isSubmitting = true;
    
    const formData = {
      ...this.transaction,
      transactionDate: new Date(this.transaction.transactionDate).toISOString()
    };

    this.transactionService.addTransaction(formData).subscribe({
      next: (response: ApiResponse<Transaction>) => {
        this.isSubmitting = false;
        this.transacctionState.triggerRefresh();
        this.formSubmit.emit(response.data);
        this.close();
        
         this.toast.show({
          message: 'Added new transaction successful!',
          type: 'success',
          duration: 3000
        });
      },
      error: (error) => {
        this.isSubmitting = false;
         this.toast.show({
          message: 'Error adding new transaction',
          type: 'error',
          duration: 3000
        });
      }
    });
  }
 
  close() {
    this.closePopup.emit();
    this.dropdownOpen.set(false);
    this.categoryDropdownOpen.set(false);
    this.initializeTransaction();
  }

}
