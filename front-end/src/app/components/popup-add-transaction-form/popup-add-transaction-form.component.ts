import {
  Component,
  computed,
  inject,
  input,
  OnInit,
  output,
  signal,
  ViewChild, // <-- IMPORTED
} from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms'; // <-- IMPORTED NgForm
import { CommonModule } from '@angular/common';
import { lastValueFrom } from 'rxjs';

import { TransactionStateService } from '../../services/transaction-state.service';
import { ToastServiceService } from '../../services/toast-service.service';
import { TransactionsService } from '../../services/transactions.service';
import {
  AddTransactionCategoryMasterDTO,
  AddTransactionTypeMasterDTO,
  Transaction,
  TransactionCategory,
  TransactionPaymentMode,
  TransactionType,
} from '../../models/login-user';
// --- NG-ZORRO-ANTD Modules ---
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzDividerModule } from 'ng-zorro-antd/divider';

@Component({
  selector: 'app-popup-add-transaction-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NzModalModule,
    NzFormModule,
    NzSelectModule,
    NzInputModule,
    NzInputNumberModule,
    NzDatePickerModule,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
  ],
  templateUrl: './popup-add-transaction-form.component.html',
  styleUrl: './popup-add-transaction-form.component.css',
})
export class PopupAddTransactionFormComponent implements OnInit {
  // --- ADD THIS LINE ---
  @ViewChild('transactionForm', { static: false }) transactionForm!: NgForm;

  // --- The rest of your component remains the same ---
  private transactionService = inject(TransactionsService);
  private toast = inject(ToastServiceService);
  private transacctionState = inject(TransactionStateService);

  isVisible = input<boolean>(false);
  transactionTypes = input<TransactionType[]>([]);
  transactionCategories = input<TransactionCategory[]>([]);
  transactionPaymentMode = input<TransactionPaymentMode[]>([]);

  typeAdded = output<TransactionType>();
  categoryAdded = output<TransactionCategory>();
  paymentModeAdded = output<TransactionPaymentMode>();
  typeRemoved = output<number>();
  categoryRemoved = output<number>();
  paymentModeRemoved = output<number>();
  closePopup = output<void>();
  formSubmit = output<any>();

  isSubmitting = false;

  typeSearchTerm = signal('');
  categorySearchTerm = signal('');
  payementModeSearchTerm = signal('');

  transaction!: Transaction;

  ngOnInit() {
    this.initializeTransaction();
  }

  filteredTransactionTypes = computed(() => {
    const term = this.typeSearchTerm().toLowerCase();
    if (!term) return this.transactionTypes();
    return this.transactionTypes().filter((type) =>
      type.transactionTypename.toLowerCase().includes(term),
    );
  });

  filteredTransactionCategories = computed(() => {
    const term = this.categorySearchTerm().toLowerCase();
    if (!term) return this.transactionCategories();
    return this.transactionCategories().filter((category) =>
      category.transactionCategoryName.toLowerCase().includes(term),
    );
  });

  filteredTransactionPaymentMode = computed(() => {
    const term = this.payementModeSearchTerm().toLowerCase();
    if (!term) return this.transactionPaymentMode();
    return this.transactionPaymentMode().filter((i) =>
      i.paymentMode.toLowerCase().includes(term),
    );
  });

  async addNewType() {
    if (!this.typeSearchTerm().trim()) return;
    const newTypeDTO: AddTransactionTypeMasterDTO = {
      transactionTypename: this.typeSearchTerm(),
      transactionTypeMasterId: 0,
      userMasterId: this.transaction.userId,
    };
    try {
      const response = await lastValueFrom(
        this.transactionService.addTransactionType(newTypeDTO),
      );
      const newType = response.data;
      this.typeAdded.emit(newType);
      this.transaction.transactionTypeMasterId =
        newType.transactionTypeMasterId;
      this.typeSearchTerm.set('');
    } catch (error) {
      console.error('Failed to add transaction type', error);
      this.toast.show({ message: 'Failed to add type', type: 'error' });
    }
  }

  async addNewCategory() {
    if (!this.categorySearchTerm().trim()) return;
    const newCategoryDTO: AddTransactionCategoryMasterDTO = {
      transactionCategoryName: this.categorySearchTerm(),
      transactionCategoryMasterId: 0,
      userMasterId: this.transaction.userId,
    };
    try {
      const response = await lastValueFrom(
        this.transactionService.addTransactionCategory(newCategoryDTO),
      );
      const newCategory = response.data;
      this.categoryAdded.emit(newCategory);
      this.transaction.transactionCategoryMasterId =
        newCategory.transactionCategoryMasterId;
      this.categorySearchTerm.set('');
    } catch (error) {
      console.error('Failed to add transaction category', error);
      this.toast.show({ message: 'Failed to add category', type: 'error' });
    }
  }

  async addPaymentMode() {
    if (!this.payementModeSearchTerm().trim()) return;
    const newPaymentDTO: TransactionPaymentMode = {
      paymentMode: this.payementModeSearchTerm(),
      userMasterId: this.transaction.userId,
      paymentModeId: null,
      isActive: 1,
      isCustom: true,
    };
    try {
      const response = await lastValueFrom(
        this.transactionService.addTransactionPayementMode(newPaymentDTO),
      );
      const newMode = response.data;
      this.paymentModeAdded.emit(newMode);
      this.transaction.transactionPaymentModeId = newMode.paymentModeId;
      this.payementModeSearchTerm.set('');
    } catch (error) {
      console.error('Failed to add payment mode', error);
      this.toast.show({ message: 'Failed to add payment mode', type: 'error' });
    }
  }

  removeType(typeId: number) {
    this.transactionService.deleteTransactionType(typeId).subscribe({
      next: () => {
        this.toast.show({
          message: 'Type removed successfully!',
          type: 'success',
        });
        this.typeRemoved.emit(typeId);
        if (this.transaction.transactionTypeMasterId === typeId) {
          this.transaction.transactionTypeMasterId = 0;
        }
      },
      error: () =>
        this.toast.show({ message: 'Error removing type', type: 'error' }),
    });
  }

  removeCategory(categoryId: number) {
    this.transactionService.deleteTransactionCategory(categoryId).subscribe({
      next: () => {
        this.toast.show({
          message: 'Category removed successfully!',
          type: 'success',
        });
        this.categoryRemoved.emit(categoryId);
        if (this.transaction.transactionCategoryMasterId === categoryId) {
          this.transaction.transactionCategoryMasterId = 0;
        }
      },
      error: () =>
        this.toast.show({ message: 'Error removing category', type: 'error' }),
    });
  }

  removePaymentMode(paymentModeId: number) {
    this.transactionService
      .deleteTransactionPaymentMode(paymentModeId)
      .subscribe({
        next: () => {
          this.toast.show({
            message: 'Payment mode removed successfully!',
            type: 'success',
          });
          this.paymentModeRemoved.emit(paymentModeId);
          if (this.transaction.transactionPaymentModeId === paymentModeId) {
            this.transaction.transactionPaymentModeId = 0;
          }
        },
        error: () =>
          this.toast.show({
            message: 'Error removing payment mode',
            type: 'error',
          }),
      });
  }

  onSubmit() {
    this.isSubmitting = true;
    const formData = {
      ...this.transaction,
      transactionDate: new Date(this.transaction.transactionDate).toISOString(),
    };
    this.transactionService.addTransaction(formData).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.transacctionState.triggerRefresh();
        this.formSubmit.emit(response.data);
        this.close();
        this.toast.show({
          message: 'Transaction added successfully!',
          type: 'success',
        });
      },
      error: () => {
        this.isSubmitting = false;
        this.toast.show({ message: 'Error adding transaction', type: 'error' });
      },
    });
  }

  close() {
    this.closePopup.emit();
    this.initializeTransaction();
  }

  initializeTransaction(): void {
    const authToken = localStorage.getItem('auth_token');
    const userId = authToken ? this.extractUserIdFromToken(authToken) : '';
    this.transaction = {
      transactionMasterId: null,
      transactionAmount: 0,
      transactionCategory: '',
      transactionCategoryMasterId: 0,
      transactionDate: new Date().toISOString().split('T')[0],
      transactionDescription: '',
      transactionNote: '',
      transactionType: '',
      transactionTypeMasterId: 0,
      transactionPaymentModeId: 0,
      transactionPaymentMode: '',
      userId: userId,
      createdAt: '',
      isActive: 1,
      deletedAt: null,
    };
  }

  private extractUserIdFromToken(token: string): string {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return (
        payload[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
        ] ||
        payload.nameidentifier ||
        payload.sub ||
        ''
      );
    } catch (e) {
      console.error('Error parsing token:', e);
      return '';
    }
  }
}
