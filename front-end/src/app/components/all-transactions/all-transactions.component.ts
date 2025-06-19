import { DatePipe, NgClass } from '@angular/common';
import { Component, inject, input, NgModule, OnInit } from '@angular/core';
import { Transaction } from '../../models/login-user';
import { TransactionsService } from '../../services/transactions.service';
import { TransactionStateService } from '../../services/transaction-state.service';
import { Subject, takeUntil } from 'rxjs';
import { FormsModule, NgModel } from '@angular/forms';

@Component({
  selector: 'app-all-transactions',
  standalone: true,
  imports: [NgClass, DatePipe , FormsModule],
  templateUrl: './all-transactions.component.html',
  styleUrl: './all-transactions.component.css'
})
export class AllTransactionsComponent implements OnInit {
  private transactionService = inject(TransactionsService);
  private transactionState = inject(TransactionStateService);
  private destroy$ = new Subject<void>();

  isLoading = true;
  userId = input<string>('');
  transactionList: Transaction[] = [];

  // Filters
  filterStartDate: string = '';
  filterEndDate: string = '';
  filterType: string = '';
  filterCategory: string = '';
  minAmount: number | null = null;
  maxAmount: number | null = null;
  searchText: string = '';

  sortColumn: string = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  categoryList: string[] = ['Food', 'Travel', 'Rent', 'Shopping', 'Bills']; // Populate based on actual data if needed

  ngOnInit(): void {
    this.getAllTransactions();
    this.transactionState.onRefresh$.subscribe(() => {
      if (this.isActive()) {
       
        this.getAllTransactions();
      }
    });
  }

  isActive(): boolean {
    return localStorage.getItem('active_tab') === 'all-transactions';
  }

  private getAllTransactions() {
    this.isLoading = true;
    this.transactionService.getAllTransactions(this.userId())
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (transactions) => {
          this.transactionList = transactions.data;
          this.isLoading = false;
          this.applySort(); // Ensure sorted after fetch
        },
        error: (error) => {
          console.error('Error loading transactions', error);
          this.isLoading = false;
        }
      });
  }

  sortTransactions(column: string) {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }
    this.applySort();
  }

  applySort() {
    if (!this.sortColumn) return;
    this.transactionList.sort((a: any, b: any) => {
      const valueA = a[this.sortColumn];
      const valueB = b[this.sortColumn];

      if (valueA == null) return 1;
      if (valueB == null) return -1;

      if (typeof valueA === 'string') {
        return this.sortDirection === 'asc'
          ? valueA.localeCompare(valueB)
          : valueB.localeCompare(valueA);
      }

      if (typeof valueA === 'number' || valueA instanceof Date) {
        return this.sortDirection === 'asc'
          ? valueA > valueB ? 1 : -1
          : valueA < valueB ? 1 : -1;
      }

      return 0;
    });
  }

  get filteredTransactions(): Transaction[] {
    return this.transactionList.filter(tx => {
      return (!this.filterStartDate || new Date(tx.transactionDate) >= new Date(this.filterStartDate)) &&
             (!this.filterEndDate || new Date(tx.transactionDate) <= new Date(this.filterEndDate)) &&
             (!this.filterType || tx.transactionType === this.filterType) &&
             (!this.filterCategory || tx.transactionCategory === this.filterCategory) &&
             (!this.minAmount || tx.transactionAmount >= this.minAmount) &&
             (!this.maxAmount || tx.transactionAmount <= this.maxAmount) &&
             (!this.searchText || tx.transactionDescription?.toLowerCase().includes(this.searchText.toLowerCase()));
    });
  }
}
