using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;

namespace Budget_Tracker_WebAPI.Repositories
{
    public interface ITransactionRepository
    {
        public  Task<TransactionDTO> AddTransaction(TransactionMaster transaction);
        public  Task<TransactionCategoryMasterDTO> AddTransactionCategory(TransactionCategoryMaster transaction);
        public  Task<TransactionTypeMasterDTO> AddTransactionType(TransactionTypeMaster transaction);

        public  Task<TransactionPaymentModeDTO> AddPaymentMode(TransactionPaymentMode model);
        public TransactionDTO EditTransaction(TransactionMaster transaction);

        public void Delete(TransactionMaster transaction);
        public void DeleteTransactionType(TransactionTypeMaster model);

        public void DeleteTransactionCategory(TransactionCategoryMaster model);


        public Task<List<TransactionDTO>> GetAllTransactionByUserID(Guid userId, int page, int pageSize);
        public Task<List<TransactionDTO>> GetRecentTransactionsByUserID(Guid userId );

        public TransactionDTO GetTransactionByTransactionMasterID(Guid transactionMasterID);
        public TransactionTypeMaster GetTransactionTypeByTransactionTypeMasterID(int transactionTypeMasterId);
        public TransactionCategoryMaster GetTransactionCategoryByTransactionCategoryMasterID(int transactionCategoryMasterId);

        public  Task<List<TransactionCategoryMasterDTO>> GetAllTransactionCategories(Guid userMasterID);

        public  Task<List<TransactionTypeMasterDTO>> GetAllTransactionTypes(Guid? userMasterID);
        public  Task<TransactionTotalDTO> GetTransactionSummary(Guid userid);
        public  Task<int> GetCountOfTransactions(Guid userid, int numberOfMonths);
        public  Task<bool> IsTransactionCategoryPresent(Guid userid, string categoryName);
        public  Task<bool> IsTransactionTypePresent(Guid userid, string typeName);
        public Task<bool> IsTransactionPaymentModePresent(Guid? userid, string payementMode);

        public Task<List<TransactionPaymentModeDTO>> GetAllTransactionPaymentMode(Guid? userMasterID);
        public TransactionPaymentMode GetTransactionPaymentModeByTransactionPayementModeMasterID(int transactionPaymentModeMasterId);
        public void DeletePaymentMode(TransactionPaymentMode model);


        Task<(List<TransactionDTO> Transactions, int TotalItems)> GetPaginatedTransactionsByUserID(Guid userId, TransactionQueryParameters queryParams);

    }
}
