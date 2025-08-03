export interface LoginUser {
  userEmail: string;
  password: string;
}

export interface UserDetails {
  userEmail: string;
  firstName: string;
  lastName: string;
  userMastrerId: string;
  userRoleId: number;
  userRole: string;
}

// TransactionType interface
export interface TransactionType {
  transactionTypeMasterId: number;
  transactionTypename: string;
  isCustom: boolean;
}

//Transaction category interface
export interface TransactionCategory {
  transactionCategoryMasterId: number;
  transactionCategoryName: string;
  isCustom: boolean;
}

export interface TransactionPaymentMode {
  paymentModeId: number | null; // Nullable number
  paymentMode: string;
  isActive: number; // sbyte in C# is a signed byte (0-255), represented as number in TypeScript
  userMasterId: string | null; // Guid in C# maps to string in TypeScript
  isCustom: boolean;
}

export interface AddTransactionTypeMasterDTO {
  transactionTypeMasterId: number | null;
  userMasterId: string; // GUID is represented as string in TypeScript
  transactionTypename: string | null; // Nullable string
}

export interface AddTransactionCategoryMasterDTO {
  transactionCategoryMasterId: number | null;
  transactionCategoryName: string; // Non-nullable (note the = null! in C#)
  userMasterId: string;
}
export interface Transaction {
  transactionMasterId: string | null;

  userId: string;
  transactionTypeMasterId: number;
  transactionCategoryMasterId: number;
  transactionDescription: string;
  transactionAmount: number;
  transactionPaymentModeId: number | null; // Nullable number for payment mode ID
  transactionPaymentMode: string; // Payment mode as a string
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
  todayIncome: string;
  weekIncome: string;
  monthIncome: string;
  dayIncomeChange: string;
  weekIncomeChange: string;
  monthIncomeChange: string;

  todayExpense: string;
  weekExpense: string;
  monthExpense: string;
  dayExpenseChange: string;
  weekExpenseChange: string;
  monthExpenseChange: string;
  
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

