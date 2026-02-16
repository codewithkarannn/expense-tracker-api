using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;

namespace Budget_Tracker_WebAPI.Repositories
{
    public interface ITransactionRepository
    {
        public  TransactionDto AddTransaction(TransactionMaster transaction);
        public  TransactionCategoryMasterDto AddTransactionCategory(TransactionCategoryMaster transaction);
        public  TransactionTypeMasterDto AddTransactionType(TransactionTypeMaster transaction);

        public  TransactionPaymentModeDto AddPaymentMode(TransactionPaymentMode model);
        public TransactionDto EditTransaction(TransactionMaster transaction);

        public void Delete(TransactionMaster transaction);
        public void DeleteTransactionType(TransactionTypeMaster model);

        public void DeleteTransactionCategory(TransactionCategoryMaster model);

        public Task<List<ExpensePieChartDto>?> GetExpensePieChartData(Guid userid , int numberofMonth);

        public Task<MonthlyExpenseOverviewDto?> GetMonthlyExpenseLineChartData(Guid userid, int numberOfMonths);
        public  Task<List<CurrencyMasterDto>?> GetAllCurrencies();
        
        public Task<PaginatedTransactionDto> GetAllTransactionByUserID(Guid userId, int page, int pageSize);
        public Task<List<TransactionDto>> GetRecentTransactionsByUserID(Guid userId );

        public TransactionDto GetTransactionByTransactionMasterID(Guid transactionMasterID);
        public TransactionTypeMaster GetTransactionTypeByTransactionTypeMasterID(int transactionTypeMasterId);
        public TransactionCategoryMaster GetTransactionCategoryByTransactionCategoryMasterID(int transactionCategoryMasterId);

        public  Task<List<TransactionCategoryMasterDto>> GetAllTransactionCategories(Guid userMasterID , int transactionTypeMasterId);

        public  Task<List<TransactionTypeMasterDto>> GetAllTransactionTypes(Guid? userMasterID);
        public  Task<TransactionTotalDto> GetTransactionSummary(Guid userid);
        public  Task<int> GetCountOfTransactions(Guid userid, int numberOfMonths);
        public  Task<bool> IsTransactionCategoryPresent(Guid userid, string categoryName);
        public  Task<bool> IsTransactionTypePresent(Guid userid, string typeName);
        public Task<bool> IsTransactionPaymentModePresent(Guid? userid, string payementMode);

        public Task<List<TransactionPaymentModeDto>> GetAllTransactionPaymentMode(Guid? userMasterID);
        public TransactionPaymentMode GetTransactionPaymentModeByTransactionPayementModeMasterID(int transactionPaymentModeMasterId);
        public void DeletePaymentMode(TransactionPaymentMode model);
        public Task<double?> GetCurrentBalance(Guid userId);
        public Task<TransactionSummaryDto?> GetTransactionSummary(Guid userid, int? numberOfMonths);

        Task<(List<TransactionDto> Transactions, int TotalItems)> GetPaginatedTransactionsByUserID(Guid userId, TransactionQueryParameters queryParams);


        #region MyRegion
        public FixedTransactionDto AddFixedTransaction(FixedTransactionMaster transaction);
        public FixedTransactionDto UpdateFixedTransaction(FixedTransactionMaster transaction);
        public FixedTransactionDto DeleteFixedTransaction(FixedTransactionMaster transaction);
        public Task<List<FixedTransactionDto>?> GetAllFixedTransactions(Guid userId);
        public Task<FixedTransactionDto?> GetFixedTransaction(Guid fixedTransactionId);

        #endregion
    }
}
