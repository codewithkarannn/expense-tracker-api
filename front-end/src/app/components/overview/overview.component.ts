import { Component, inject, Inject, input, OnInit } from '@angular/core';
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
  // Define your properties and methods here
  ngOnInit() {
    // Initialization logic here
    this.getAllTransactions();
    this.getTransactionCount()
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
}
