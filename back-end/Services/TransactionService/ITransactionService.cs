using Budget_Tracker_WebAPI.DTOs;


namespace Budget_Tracker_WebAPI.Services
{
    public interface  ITransactionService
    {
        public Task<List<ExpensePieChartDto>?> GetExpensePieChartData(Guid userid  , int numberOfMonths);
        public Task<MonthlyExpenseOverviewDto?> GetMonthlyExpenseLineChartData(Guid userid, int numberOfMonths);
        public  TransactionDto AddTransaction(CreateTransactionDto transactionDto);
        public  Task<TransactionTypeMasterDto> AddTransactionType(AddTransactionTypeMasterDto transactionDto);
        public Task<TransactionCategoryMasterDto> AddTransactionCategory(AddTransactionCategoryMasterDto modelDto);
        public TransactionDto EditTransaction(EditTransactionDto transactionDto);
        public Task<List<CurrencyMasterDto>?> GetAllCurrencies();
        public  Task<PaginatedTransactionDto> GetAllTransactionsByUserId(Guid userId , int page , int pageSize );
        public Task<List<TransactionDto>> GetRecentTransactionsByUserId(Guid userId);
        public void DeleteTransaction(Guid transactionMasterId);
        public  void DeleteTransactionType(int transactionTypeMasterId);
        public void DeleteTransactionCategory(int transactionCategoryMasterId);
        public TransactionDto GetTransactionByTransactionId(Guid transactionMasterId);
        public Task<List<TransactionCategoryMasterDto>> GetAllTransactionCategories(Guid userMasterId , int transactionTypeMasterId);

        public Task<List<TransactionTypeMasterDto>> GetAllTransactionTypes(Guid? userMasterId);
        public Task<TransactionTotalDto> GetTransactionSummary(Guid userId);
        
        public Task<TransactionSummaryDto?> GetTransactionSummary(Guid userId , int? numberOfMonths);
        public Task<int> GetCountOfTransactions(Guid userId, int numberOfMonths);

        public Task<List<TransactionPaymentModeDto>> GetAllPaymentMode(Guid? userMasterId);
        public void DeleteTransactionPayementMode(int paymentModeMasterId);
        public Task<TransactionPaymentModeDto> AddTransactionPayementMode(TransactionPaymentModeDto modelDto);

        Task<(List<TransactionDto> Transactions, int TotalItems)> GetPaginatedTransactionsByUserId(Guid userId, TransactionQueryParameters queryParams);

        #region Fixed Trasactions

        public FixedTransactionDto AddFixedTransaction(CreateFixedTransactionDto transactionDto);

        #endregion
    }
}
