import { CommonModule, DatePipe, NgClass } from '@angular/common';
import { Component, inject, input, NgModule, OnInit } from '@angular/core';
import { Transaction, TransactionCategory, TransactionPaymentMode, TransactionType } from '../../models/login-user';
import { TransactionsService } from '../../services/transactions.service';
import { TransactionStateService } from '../../services/transaction-state.service';
import { debounceTime, distinctUntilChanged, Subject, takeUntil } from 'rxjs';
import { FormsModule, NgModel } from '@angular/forms';
import { PaginatedResponse, TransactionRequestParams } from '../../models/api-reponse';

@Component({
  selector: 'app-all-transactions',
  standalone: true,
  imports: [NgClass, DatePipe, FormsModule, CommonModule],
  templateUrl: './all-transactions.component.html',
  styleUrl: './all-transactions.component.css'
})
export class AllTransactionsComponent implements OnInit {
  private transactionService = inject(TransactionsService);
  private transactionState = inject(TransactionStateService);
  private destroy$ = new Subject<void>();
  private searchSubject = new Subject<string>();

  isLoading = true;
  userId = input<string>('');
  transactionTypes = input<TransactionType[]>();
  transactionCategories = input<TransactionCategory[]>();
  transactionPaymentMode = input<TransactionPaymentMode[]>([]);
  transactionList: Transaction[] = [];
  private colorPalette = [
    { bg: 'bg-blue-100', text: 'text-blue-800' },
    { bg: 'bg-green-100', text: 'text-green-800' },
    { bg: 'bg-purple-100', text: 'text-purple-800' },
    { bg: 'bg-yellow-100', text: 'text-yellow-800' },
    { bg: 'bg-indigo-100', text: 'text-indigo-800' },
    { bg: 'bg-pink-100', text: 'text-pink-800' },
    { bg: 'bg-teal-100', text: 'text-teal-800' },
  ];
  private categoryColorMap = new Map<number, { bg: string, text: string }>();
  // Filters
  filterStartDate: string = '';
  filterEndDate: string = '';
  filterType: string = '';
  filterCategory: string = '';
  minAmount: number | null = null;
  maxAmount: number | null = null;
  searchText: string = '';

  sortColumn: string = '';
  sortDirection: 'asc' | 'desc' = 'desc';
   currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  totalPages = 0;



  ngOnInit(): void {
    // this.getAllTransactions();
    // this.transactionState.onRefresh$.subscribe(() => {
    //   if (this.isActive()) {

    //     this.getAllTransactions();
    //   }
    // });
    this.fetchTransactions(); // Initial data fetch

    this.transactionState.onRefresh$.pipe(takeUntil(this.destroy$)).subscribe(() => {
      if (this.isActive()) { this.fetchTransactions(); }
    });

    // Debounce search input to avoid excessive API calls on every keystroke
    this.searchSubject.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      
      takeUntil(this.destroy$)
    ).subscribe(() => this.onFilterChange());
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

  // sortTransactions(column: string) {
  //   if (this.sortColumn === column) {
  //     this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
  //   } else {
  //     this.sortColumn = column;
  //     this.sortDirection = 'asc';
  //   }
  //   this.applySort();
  // }

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
    public getCategoryStyles(categoryId?: number): { [key: string]: boolean } {
    if (categoryId === undefined || categoryId === null) {
      return { 'bg-gray-100': true, 'text-gray-800': true };
    }
    if (this.categoryColorMap.has(categoryId)) {
      const colors = this.categoryColorMap.get(categoryId)!;
      return { [colors.bg]: true, [colors.text]: true };
    }
    const colorIndex = this.categoryColorMap.size % this.colorPalette.length;
    const newColor = this.colorPalette[colorIndex];
    this.categoryColorMap.set(categoryId, newColor);
    return { [newColor.bg]: true, [newColor.text]: true };
  }


    fetchTransactions(): void {
    this.isLoading = true;
    const params: TransactionRequestParams = {
      Page: this.currentPage,
      PageSize: this.pageSize,
      SortColumn: this.sortColumn,
      SortDirection: this.sortDirection,
      FilterType: this.filterType,
      FilterCategory: this.filterCategory,
      StartDate: this.filterStartDate,
      EndDate: this.filterEndDate,
      MinAmount: this.minAmount,
      MaxAmount: this.maxAmount,
      SearchText: this.searchText
    };

    // MODIFIED: Call the new paginated service method
    this.transactionService.getAllPaginatedTransactions(this.userId(), params)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response: PaginatedResponse<Transaction>) => {
          this.transactionList = response.data;
          this.currentPage = response.currentPage;
          this.pageSize = response.pageSize;
          this.totalItems = response.totalItems;
          this.totalPages = response.totalPages;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading transactions', error);
          this.isLoading = false;
        }
      });

      
  }

    sortTransactions(column: string): void {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }
    this.fetchTransactions();
  }

   onFilterChange(): void {
    this.currentPage = 1;
    this.fetchTransactions();
  }
 onSearchChange(): void {
    this.searchSubject.next(this.searchText);
  }
 goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
      this.currentPage = page;
      this.fetchTransactions();
    }
  }

    onPageSizeChange(): void {
    this.currentPage = 1; // Reset to page 1 is crucial
    this.fetchTransactions();
  }
  get paginationPages(): number[] {
    // This is a simple implementation. For many pages, you'd want a more complex one.
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

}
