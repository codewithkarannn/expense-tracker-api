import {
  AfterViewInit,
  Component,
  ElementRef,
  inject,
  Inject,
  input,
  OnInit,
  output,
  ViewChild,
} from '@angular/core';
import { TransactionsService } from '../../services/transactions.service';
import { Transaction } from '../../models/login-user';
import { CommonModule, DatePipe, DecimalPipe, NgClass } from '@angular/common';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NgModel } from '@angular/forms';
import Chart from 'chart.js/auto';
import { ExpenseIncomeLineGraphComponent } from '../expense-income-line-graph/expense-income-line-graph.component';
import { NzIconModule } from 'ng-zorro-antd/icon';

import {
  LucideAngularModule,
  LucideIconData,
  LucideIconProvider,
  Utensils,
  Banknote,
  CarIcon,
  HouseIcon,
  RectangleEllipsisIcon,
  ShoppingBagIcon,
  EllipsisVerticalIcon,
  TrashIcon,
  TvMinimalIcon,
  BanknoteXIcon,
} from 'lucide-angular';
import { DragDropModule } from '@angular/cdk/drag-drop'; // <-- Import this
import {
  trigger,
  state,
  style,
  transition,
  animate,
} from '@angular/animations';
import { TransactionCardComponent } from '../transaction-card/transaction-card.component'; // <-- Import animations
@Component({
  selector: 'app-overview',
  imports: [
    DatePipe,
    NgClass,
    ExpenseIncomeLineGraphComponent,
    DecimalPipe,
    NzDropDownModule,
    NzMenuModule,
    CommonModule,
    DragDropModule,
    LucideAngularModule,
    NzIconModule,
    NzModalModule,
    TransactionCardComponent,
  ],

  templateUrl: './overview.component.html',
  styleUrl: './overview.component.css',
})
export class OverviewComponent implements OnInit {
  private transactionService = inject(TransactionsService);
  transactionList: Transaction[] = [];
  transactionCount: number = 0;
  isLoading: boolean = true;
  userId = input<string>('');
  utensilsIcon = Utensils;
  banknoteIcon = Banknote;
  trashIcon = BanknoteXIcon;
  ellipsisIcon = EllipsisVerticalIcon;
  carIcon = CarIcon;
  navigateToAllTransaction = output<void>();
  public chartData = {
    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May'],

    datasets: [
      {
        label: 'Expense',
        data: [120, 150, 180, 90, 210],
        borderColor: 'green',
        tension: 0.1,
      },
    ],
  };

  // Property to hold the chart options
  public chartOptions = {
    responsive: true,
    maintainAspectRatio: false,
  };

  private colorPalette = [
    { bg: 'bg-blue-100', text: 'text-blue-800' },
    { bg: 'bg-green-100', text: 'text-green-800' },
    { bg: 'bg-purple-100', text: 'text-purple-800' },
    { bg: 'bg-yellow-100', text: 'text-yellow-800' },
    { bg: 'bg-indigo-100', text: 'text-indigo-800' },
    { bg: 'bg-pink-100', text: 'text-pink-800' },
    { bg: 'bg-teal-100', text: 'text-teal-800' },
  ];

  private categoryColorMap = new Map<number, { bg: string; text: string }>();

  // Define your properties and methods here
  ngOnInit() {
    // Initialization logic here
    this.getAllTransactions();
    this.getTransactionCount();
  }

  getTransactionIcon(transaction: Transaction): LucideIconData {
    if (transaction.transactionTypeMasterId == 1) {
      return this.banknoteIcon;
    } else if (transaction.transactionTypeMasterId == 2) {
      if (transaction.transactionCategory == 'Food & Dining ') {
        return this.utensilsIcon;
      }
      if (transaction.transactionCategory == 'Transportation') {
        return this.carIcon;
      }
      if (transaction.transactionCategory == 'Housing') {
        return HouseIcon;
      }
      if (transaction.transactionCategory == 'Shopping') {
        return ShoppingBagIcon;
      }
      if (transaction.transactionCategory == 'Entertainment') {
        return TvMinimalIcon;
      }
    }

    return RectangleEllipsisIcon;
  }
  getBgColor(transaction: Transaction): string {
    if (transaction.transactionTypeMasterId == 1) {
      return 'bg-green-500';
    } else if (transaction.transactionTypeMasterId == 2) {
      if (transaction.transactionCategory == 'Food & Dining ') {
        return 'bg-yellow-500';
      }
      if (transaction.transactionCategory == 'Transportation') {
        return 'bg-blue-500';
      }
      if (transaction.transactionCategory == 'Housing') {
        return 'bg-purple-500';
      }
      if (transaction.transactionCategory == 'Shopping') {
        return 'bg-orange-500';
      }
      if (transaction.transactionCategory == 'Entertainment') {
        return 'bg-red-500';
      }
    }

    return 'bg-gray-500';
  }

  deleteTransaction(transactionMaster: Transaction) {
    console.log('Deleting transaction:', transactionMaster);
    if (transactionMaster.transactionMasterId !== null) {
      this.transactionService
        .deleteTransaction(transactionMaster.transactionMasterId)
        .subscribe({
          next: (response) => {
            if (response.success) {
              this.getAllTransactions();
            }
          },
        });
    }
  }

  getAllTransactions() {
    if (this.userId != null && this.userId != undefined) {
      this.transactionService.getRecentTransaction(this.userId()).subscribe({
        next: (response) => {
          if (response.success) {
            this.transactionList = response.data;
            this.isLoading = false; // Set loading to false after data is fetched
          }
        },
        error: (error) => {
          console.error('Error fetching transactions:', error);
        },
      });
    }
  }

  navigateToAllTransactions() {
    console.log('Navigating to all transactions');
    this.navigateToAllTransaction.emit();
  }

  getTransactionCount() {
    if (this.userId != null && this.userId != undefined) {
      this.transactionService.getTransactionCount(this.userId(), 1).subscribe({
        next: (response) => {
          if (response.success) {
            this.transactionCount = response.data;
          }
        },
        error: (error) => {
          console.error('Error fetching transactions:', error);
        },
      });
    }
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
}
