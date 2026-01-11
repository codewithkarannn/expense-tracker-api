using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;
using Budget_Tracker_WebAPI.Repositories;

namespace Budget_Tracker_WebAPI.Services
{
    public interface  ITransactionService 
    {
        public  TransactionDto AddTransaction(CreateTransactionDto transactionDTO);
        public  Task<TransactionTypeMasterDTO> AddTransactionType(AddTransactionTypeMasterDTO transactionDTO);
        public Task<TransactionCategoryMasterDTO> AddTransactionCategory(AddTransactionCategoryMasterDTO modelDTO);
        public TransactionDto EditTransaction(EditTransactionDTO transactionDTO);
        public  Task<List<TransactionDto>> GetAllTransactionsByUserID(Guid userID , int page , int pageSize );
        public Task<List<TransactionDto>> GetRecentTransactionsByUserID(Guid userID);
        public void DeleteTransaction(Guid transactionMasterID);
        public  void DeleteTransactionType(int transactionTypeMasterID);
        public void DeleteTransactionCategory(int transactionCategoryMasterID);
        public TransactionDto GetTransactionByTransactionID(Guid transactionMasterID);
        public Task<List<TransactionCategoryMasterDTO>> GetAllTransactionCategories(Guid userMasterID);

        public Task<List<TransactionTypeMasterDTO>> GetAllTransactionTypes(Guid? userMasterID);
        public Task<TransactionTotalDTO> GetTransactionSummary(Guid userId);
        public Task<int> GetCountOfTransactions(Guid userId, int numberOfMonths);

        public Task<List<TransactionPaymentModeDTO>> GetAllPaymentMode(Guid? userMasterID);
        public void DeleteTransactionPayementMode(int paymentModeMasterId);
        public Task<TransactionPaymentModeDTO> AddTransactionPayementMode(TransactionPaymentModeDTO modelDTO);

        Task<(List<TransactionDto> Transactions, int TotalItems)> GetPaginatedTransactionsByUserID(Guid userId, TransactionQueryParameters queryParams);
    }
}
