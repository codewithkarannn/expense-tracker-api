export interface LoginUser {
  userEmail: string;
  password: string;
}

// TransactionType interface
export interface TransactionType {
  transactionTypeMasterId: number;
  transactionTypename: string;

}

//Transaction category interface
export interface TransactionCategory {
  transactionCategoryMasterId: number;
  transactionCategoryName: string;
}

export interface Transaction {
  transactionMasterId: string | null;
  userId: string;
  transactionTypeMasterId: number;
  transactionCategoryMasterId: number;
  transactionDescription: string;
  transactionAmount: number;
  transactionDate: Date | string;
  transactionNote: string;
  isActive: number;
  transactionType: string;
  transactionCategory: string;
  createdAt: Date | string;
  deletedAt?: Date | null ;
}


/**
 * Financial summary for a user's transactions
 */
export interface TransactionSummary {
  /** Unique user identifier */
  userId: string;
  
  /** Current account balance */
  currentBalance: string;
  
  /** Previous period's balance */
  previousBalance: string;
  
  /** Percentage change in balance (current vs previous) */
  balancePercentageChange: string;
  
  /** Total expenses in current period */
  currentExpense: string;
  
  /** Total expenses in previous period */
  previousExpense: string;
  
  /** Percentage change in expenses */
  expensePercentageChange: string;
  
  /** Total income in current period */
  currentIncome: string;
  
  /** Total income in previous period */
  previousIncome: string;
  
  /** Percentage change in income */
  incomePercentageChange: string;
}