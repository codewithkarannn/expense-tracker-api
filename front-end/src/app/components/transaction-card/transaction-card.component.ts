import { Component, input, OnInit, output } from '@angular/core';
import { Transaction } from '../../models/login-user';
import {
  BanknoteIcon,
  BanknoteXIcon,
  CarIcon,
  HouseIcon,
  LucideAngularModule,
  LucideIconData,
  RectangleEllipsis,
  RectangleEllipsisIcon,
  ShoppingBagIcon,
  TvMinimalIcon,
  Utensils,
} from 'lucide-angular';
import { CommonModule, DatePipe, DecimalPipe, NgClass } from '@angular/common';
import { ExpenseIncomeLineGraphComponent } from '../expense-income-line-graph/expense-income-line-graph.component';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { CdkDragEnd, DragDropModule } from '@angular/cdk/drag-drop';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalModule } from 'ng-zorro-antd/modal';

import {
  trigger,
  state,
  style,
  transition,
  animate,
} from '@angular/animations'; // <-- Import animations

@Component({
  selector: 'app-transaction-card',
  imports: [
    LucideAngularModule,
    DatePipe,
    DragDropModule,
    NgClass,
    ExpenseIncomeLineGraphComponent,
    DecimalPipe,
    NzDropDownModule,
    NzMenuModule,
    CommonModule,
    DragDropModule,
    NzIconModule,
    NzModalModule,
  ],
  animations: [
    trigger('swipeAnimation', [
      state('initial', style({ transform: 'translateX(0)', opacity: 1 })),
      state('swiped', style({ transform: 'translateX(-100%)', opacity: 0 })),
      transition('initial => swiped', [animate('300ms ease-out')]),
    ]),
  ],
  templateUrl: './transaction-card.component.html',
  styleUrl: './transaction-card.component.css',
})
export class TransactionCardComponent implements OnInit {
  transaction = input.required<Transaction>();
  deleteTransaction = output<Transaction>();
  trashIcon = BanknoteXIcon;
  public isDragDisabled = false;

  ngOnInit(): void {
    this.isDragDisabled = navigator.maxTouchPoints > 0; // Initialize drag state
  }
  // The current position of the draggable card
  dragPosition = { x: 0, y: 0 };

  // The state for our animation trigger
  animationState: 'initial' | 'swiped' = 'initial';

  // A threshold in pixels. If swiped more than this, we trigger delete.
  readonly SWIPE_THRESHOLD = -100; // Swipe left is a negative value

  onDragEnd(event: CdkDragEnd) {
    // Get the horizontal distance the user dragged
    const distance = event.distance.x;

    if (distance < this.SWIPE_THRESHOLD) {
      // Swiped far enough to the left: trigger the delete animation
      this.animationState = 'swiped';
    } else {
      // Not swiped far enough: snap back to the original position
      event.source.reset(); // This is a handy CDK method!
      this.dragPosition = { x: 0, y: 0 };
    }
  }

  // This function is called when the 'swipeAnimation' is finished
  onAnimationDone(event: any) {
    // We only care about when the 'swiped' animation finishes
    if (this.animationState === 'swiped') {
      this.onDelete();
    }
  }
  onDelete() {
    this.deleteTransaction.emit(this.transaction());
  }
  utensilsIcon = Utensils;

  getTransactionIcon(transaction: Transaction): LucideIconData {
    if (transaction.transactionTypeMasterId == 1) {
      return BanknoteIcon;
    } else if (transaction.transactionTypeMasterId == 2) {
      if (transaction.transactionCategory == 'Food & Dining ') {
        return this.utensilsIcon;
      }
      if (transaction.transactionCategory == 'Transportation') {
        return CarIcon;
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
}
