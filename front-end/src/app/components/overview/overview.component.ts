import { Component, inject, Inject, input, OnInit, output } from '@angular/core';
import { TransactionsService } from '../../services/transactions.service';
import { Transaction } from '../../models/login-user';
import { DatePipe, NgClass } from '@angular/common';
import { NgModel } from '@angular/forms';

@Component({
  selector: 'app-overview',
  imports: [DatePipe , NgClass],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.css'
})
export class OverviewComponent implements OnInit {
  private transactionService  =  inject(TransactionsService);
  transactionList : Transaction[] = [];
  transactionCount : number = 0;
  isLoading: boolean = true;
  userId = input<string>('');
  navigateToAllTransaction = output<void>();

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


  // Define your properties and methods here
  ngOnInit() {
    // Initialization logic here
    this.getAllTransactions();
    this.getTransactionCount()
  }


  deleteTransaction(transactionMasterId : string){
    
    this.transactionService.deleteTransaction(transactionMasterId).subscribe({
      next: (response) => {
        if(response.success)
        {
            this.getAllTransactions();
        }
      }
    })

  }
 
  getAllTransactions() {  
    if(this.userId != null && this.userId != undefined)
    {
      this.transactionService.getRecentTransaction(this.userId()).subscribe(
        {
          next: (response) => {
            if (response.success) {
              this.transactionList = response.data;
              this.isLoading = false; // Set loading to false after data is fetched
            }
          },
          error: (error) => {
            console.error("Error fetching transactions:", error);
          }
        }
      );
    }
      
  }

  navigateToAllTransactions(){
    console.log("Navigating to all transactions");
    this.navigateToAllTransaction.emit();
  }
  getTransactionCount() {  
    if(this.userId != null && this.userId != undefined)
    {
      this.transactionService.getTransactionCount(this.userId(), 1).subscribe(
        {
          next: (response) => {
            if (response.success) {
              this.transactionCount = response.data;
            }
          },
          error: (error) => {
            console.error("Error fetching transactions:", error);
          }
        }
      );
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
