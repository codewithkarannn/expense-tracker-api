using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;
using Budget_Tracker_WebAPI.Repositories;


namespace Budget_Tracker_WebAPI.Services
{
    public class TransactionService(ITransactionRepository transactionRepository) : ITransactionService
    {
        public async Task<List<ExpensePieChartDto>?> GetExpensePieChartData(Guid userid , int numberOfMonths)
        {
            try
            {
                return  await  transactionRepository.GetExpensePieChartData(userid, numberOfMonths);
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error  fetching the data . Please try again.\"", ex); ;
            }
        }
        
        
        
        public async Task<List<CurrencyMasterDto>?> GetAllCurrencies()
        {
            try
            {
                return  await  transactionRepository.GetAllCurrencies();
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error  fetching the data . Please try again.\"", ex); ;
            }
        }

        public async Task<MonthlyExpenseOverviewDto?> GetMonthlyExpenseLineChartData(Guid userid , int numberOfMonths)
        {
            try
            {
                return  await  transactionRepository.GetMonthlyExpenseLineChartData(userid, numberOfMonths);
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error  fetching the data . Please try again.\"", ex); ;
            }
        }
        
        
        public TransactionDto AddTransaction(CreateTransactionDto transactionDto)
        {
            try
            {

                var newTransaction = new TransactionMaster
                {
                    TransactionTypeMasterId = transactionDto.TransactionTypeMasterId,
                    TransactionAmount = transactionDto.TransactionAmount,
                    TransactionCategoryMasterId = transactionDto.TransactionCategoryMasterId,
                    TransactionDescription = transactionDto.TransactionDescription,
                    TransactionDate = transactionDto.TransactionDate,
                    TransactionNote = transactionDto.TransactionNote,
                    TransactionPaymentmodeId =  transactionDto.TransactionPaymentModeId,
                    CreatedAt = System.DateTime.UtcNow,
                    IsActive = 1,
                    UserId = transactionDto.UserId
                };

                return  transactionRepository.AddTransaction(newTransaction);
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error adding new  transaction. Please try again.\"", ex); ;
            }
        }

        public async Task<TransactionTypeMasterDto> AddTransactionType(AddTransactionTypeMasterDto modelDto)
        {   
            try
            {       
                var isPresent = await  transactionRepository.IsTransactionTypePresent(modelDto.UserMasterId, modelDto?.TransactionTypename);

                if(isPresent != true)
                {

                    var newModel = new TransactionTypeMaster
                    {
                        
                        TransactionTypename = modelDto.TransactionTypename,
                        IsActive = 1,
                        UserMasterId = modelDto.UserMasterId
                    };

                    return  transactionRepository.AddTransactionType(newModel);
                }
                throw new Exception("Transaction type already present");
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error adding new  transaction. Please try again.\"", ex); ;
            }
        }

        public async Task<TransactionCategoryMasterDto> AddTransactionCategory(AddTransactionCategoryMasterDto modelDTO)
        {
            try
            {
                var IsPresent = await transactionRepository.IsTransactionCategoryPresent(modelDTO.UserMasterId, modelDTO?.TransactionCategoryName);

                if (IsPresent != true)
                {

                    var newModel = new TransactionCategoryMaster
                    {
                        
                        TransactionCategoryName = modelDTO.TransactionCategoryName,
                        IsActive = 1,
                        UserMasterId = modelDTO.UserMasterId
                    };

                    return  transactionRepository.AddTransactionCategory(newModel);
                }
                throw new Exception("Transaction category already present");
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error adding new  transaction. Please try again.\"", ex); ;
            }
        }

        public async Task<TransactionPaymentModeDto> AddTransactionPayementMode(TransactionPaymentModeDto modelDto)
        {
            try
            {

                var IsPresent = await transactionRepository.IsTransactionPaymentModePresent(modelDto?.UserMasterId ?? null, modelDto?.PaymentMode);

                if (IsPresent != true)
                {

                    var newModel = new TransactionPaymentMode
                    {

                        
                        IsActive = 1,
                        PaymentMode = modelDto.PaymentMode,
                        
                        UserMasterId = modelDto.UserMasterId
                    };

                    return  transactionRepository.AddPaymentMode(newModel);
                }
                throw new Exception("Transaction payment mode already present");
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error adding new  payment mode. Please try again.\"", ex); ;
            }
        }

        public TransactionDto EditTransaction(EditTransactionDto transactionDto)
        {
            try
            {
                if(transactionDto.TransactionMasterId  != Guid.Empty)
                {
                    var existingTransaction =   transactionRepository.GetTransactionByTransactionMasterID(transactionDto.TransactionMasterId);

                    if (existingTransaction != null)
                    {
                        var newTransaction = new TransactionMaster
                        {
                            TransactionTypeMasterId = transactionDto.TransactionTypeMasterId,
                            TransactionAmount = transactionDto.TransactionAmount,
                            TransactionCategoryMasterId = transactionDto.TransactionCategoryMasterId,
                            TransactionDescription = transactionDto.TransactionDescription,
                            TransactionDate = System.DateTime.UtcNow,
                            TransactionPaymentmodeId = transactionDto.TransactionPaymentModeId,
                            TransactionNote = transactionDto.TransactionNote,
                            TransactionMasterId = transactionDto.TransactionMasterId,

                            UserId = transactionDto.UserId
                        };

                        return transactionRepository.EditTransaction(newTransaction);

                    }
                    else
                    {


                        throw new KeyNotFoundException("Transaction not found.");

                    }
                }
                else
                {


                    throw new KeyNotFoundException("Transaction  not found.");

                }

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error editing  new  transaction. Please try again.\"", ex); ;
            }
        }

        public async Task<PaginatedTransactionDto> GetAllTransactionsByUserId(Guid userID, int page, int pageSize)
        {
            try
            {

                var transactions = await transactionRepository.GetAllTransactionByUserID(userID,  page,  pageSize);
                if (transactions.TransactionList.Count > 0)
                {
                    return transactions;
                }
                else
                {
                    throw new KeyNotFoundException("No transactions found ");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching new  transaction. Please try again.\"", ex); ;
            }
        }


        public async Task<List<TransactionDto>> GetRecentTransactionsByUserId(Guid userID)
        {
            try
            {

                var transactions = await transactionRepository.GetRecentTransactionsByUserID(userID );
                if (transactions.Count > 0)
                {
                    return transactions;
                }
                else
                {
                    throw new KeyNotFoundException("No transactions found ");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching new  transaction. Please try again.\"", ex); ;
            }
        }

        public async void DeleteTransaction(Guid transactionMasterID)
        {
            try
            {
                var existingTransaction =    transactionRepository.GetTransactionByTransactionMasterID(transactionMasterID);

                if (existingTransaction != null)
                {
                    var selectedTransaction = new TransactionMaster
                    {
                        TransactionTypeMasterId = existingTransaction.TransactionTypeMasterId,
                        TransactionAmount = existingTransaction.TransactionAmount,
                        TransactionCategoryMasterId = existingTransaction.TransactionCategoryMasterId,
                        TransactionDescription = existingTransaction.TransactionDescription,
                        TransactionDate = existingTransaction.TransactionDate,
                        TransactionNote = existingTransaction.TransactionNote,
                        TransactionMasterId = existingTransaction.TransactionMasterId ?? Guid.Empty,
                        IsActive = 0,
                        UserId = existingTransaction.UserId
                    };

                     transactionRepository.Delete(selectedTransaction);

                }
                else
                {


                    throw new KeyNotFoundException("Transaction not found.");

                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error deleting  new  transaction. Please try again.\"", ex); ;
            }
        }

        public async void DeleteTransactionType(int transactionTypeMasterID)
        {
            try
            {
                var existingTransaction =  transactionRepository.GetTransactionTypeByTransactionTypeMasterID(transactionTypeMasterID);

                if (existingTransaction != null)
                {
                    var selectedTransaction = new TransactionTypeMaster
                    {
                        TransactionTypeMasterId = existingTransaction.TransactionTypeMasterId,
                        TransactionTypename = existingTransaction.TransactionTypename,
                        IsActive = 0,
                        UserMasterId = existingTransaction.UserMasterId,
                    };

                    transactionRepository.DeleteTransactionType(selectedTransaction);

                }
                else
                {


                    throw new KeyNotFoundException("Transaction type not found.");

                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error deleting    transaction type. Please try again.\"", ex); ;
            }
        }

        public async void DeleteTransactionCategory(int transactionCategoryMasterID)
        {
            try
            {
                var existingTransaction =  transactionRepository.GetTransactionCategoryByTransactionCategoryMasterID(transactionCategoryMasterID);

                if (existingTransaction != null)
                {
                    var selectedTransaction = new TransactionCategoryMaster
                    {
                        TransactionCategoryMasterId = existingTransaction.TransactionCategoryMasterId,
                        TransactionCategoryName = existingTransaction.TransactionCategoryName,

                        IsActive = 0,
                        UserMasterId = existingTransaction.UserMasterId,
                    };

                    transactionRepository.DeleteTransactionCategory(selectedTransaction);

                }
                else
                {


                    throw new KeyNotFoundException("Transaction type not found.");

                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error deleting    transaction type. Please try again.\"", ex); ;
            }
        }


        public async void DeleteTransactionPayementMode(int paymentModeMasterId)
        {
            try
            {
                var existingModel = transactionRepository.GetTransactionPaymentModeByTransactionPayementModeMasterID(paymentModeMasterId);

                if (existingModel != null)
                {
                    var selectedModel = new TransactionPaymentMode
                    {
                        PaymentModeId = existingModel.PaymentModeId,
                        PaymentMode= existingModel.PaymentMode,

                        IsActive = 0,
                        UserMasterId = existingModel.UserMasterId,
                    };

                    transactionRepository.DeletePaymentMode(selectedModel);

                }
                else
                {


                    throw new KeyNotFoundException("Transaction payement mode not found.");

                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error deleting    transaction payment mode. Please try again.\"", ex); ;
            }
        }


        public TransactionDto GetTransactionByTransactionId(Guid transactionMasterID)
        {
            try
            {

                var transaction = transactionRepository.GetTransactionByTransactionMasterID(transactionMasterID);
                if (transaction != null)
                {
                    return transaction;
                }
                else
                {
                    throw new KeyNotFoundException("No transaction found ");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching   transaction. Please try again.\"", ex); ;
            }
        }

        public async Task<List<TransactionTypeMasterDto>> GetAllTransactionTypes(Guid? userMasterID)
        {
            try
            {

                var transactionTypes = await transactionRepository.GetAllTransactionTypes(userMasterID);

              

                if (transactionTypes != null)
                {
                    var transactionTypesDto = transactionTypes.Select(i => new TransactionTypeMasterDto
                    {
                        TransactionTypeMasterId =  i.TransactionTypeMasterId,
                        TransactionTypename = i.TransactionTypename,
                        IsCustom = i.IsCustom
                        
                    }).ToList();

                    return transactionTypesDto;
                }
                else
                {
                    throw new KeyNotFoundException("No transaction types  found ");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching   transaction types. Please try again.\"", ex); ;
            }
        }

        public async Task<List<TransactionCategoryMasterDto>> GetAllTransactionCategories(Guid userMasterID , int transactionTypeMasterId)
        {
            try
            {

                var transactionCat = await transactionRepository.GetAllTransactionCategories(userMasterID , transactionTypeMasterId);
                if (transactionCat != null)
                {
                    var transactionCategoryDTO = transactionCat.Select(i => new TransactionCategoryMasterDto
                    {
                        TransactionCategoryMasterId =  i.TransactionCategoryMasterId,
                        TransactionCategoryName = i.TransactionCategoryName,
                        IsCustom = i.IsCustom

                    }).ToList();
                    return transactionCategoryDTO;
                }
                else
                {
                    throw new KeyNotFoundException("No transaction categories  found ");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching   transaction categories. Please try again.\"", ex); ;
            }
        }

        public async Task<List<TransactionPaymentModeDto>> GetAllPaymentMode(Guid? userMasterID)
        {
            try
            {
                var paymentModes = await transactionRepository.GetAllTransactionPaymentMode(userMasterID);
                {
                    var paymentModeList = paymentModes.Select(i => new TransactionPaymentModeDto
                    {
                        PaymentModeId = i.PaymentModeId,
                        UserMasterId = i.UserMasterId,
                        IsCustom = i.IsCustom,
                        IsActive = i.IsActive,
                        PaymentMode = i.PaymentMode,
                        

                    }).ToList();
                    return paymentModeList;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching   transaction payment mode. Please try again.\"", ex); ;
            }
        }


        public async Task<TransactionTotalDto> GetTransactionSummary(Guid userId)
        {
            try
            {

                var transaction = await transactionRepository.GetTransactionSummary(userId);
                if (transaction != null)
                {
                    return transaction;
                }
                else
                {
                    throw new KeyNotFoundException("No transaction found ");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching   transaction. Please try again.\"", ex); ;
            }
        }

        
        public async Task<TransactionSummaryDto?> GetTransactionSummary(Guid userId , int? numberOfMonths)
        {
            try
            {

                var transaction = await transactionRepository.GetTransactionSummary(userId, numberOfMonths);
                if (transaction != null)
                {
                    return transaction;
                }
                else
                {
                    throw new KeyNotFoundException("No transaction found ");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching   transaction. Please try again.\"", ex); ;
            }
        }


        public async Task<int> GetCountOfTransactions(Guid userId , int numberOfMonths)
        {
            try
            {

                var transactionCount = await transactionRepository.GetCountOfTransactions(userId, numberOfMonths);
                if (transactionCount != null)
                {
                    return transactionCount;
                }
                else
                {
                    throw new KeyNotFoundException("No transaction found ");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching   transaction count. Please try again.\"", ex); ;
            }
        }

        public async Task<(List<TransactionDto> Transactions, int TotalItems)> GetPaginatedTransactionsByUserId(Guid userId, TransactionQueryParameters queryParams)
        {
            try
            {
                var transactions = await transactionRepository.GetPaginatedTransactionsByUserID(userId, queryParams);

                return transactions;
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching   transaction . Please try again.\"", ex); ;
            }
        }
        
        
        #region FixedTransaction
        
        public FixedTransactionDto AddFixedTransaction(CreateFixedTransactionDto transactionDto)
        {
            try
            {

                var newTransaction = new FixedTransactionMaster
                {
                    TransactionTypeMasterId = transactionDto.TransactionTypeMasterId,
                    TransactionAmount = (decimal) transactionDto.TransactionAmount,
                    TransactionCategoryMasterId = transactionDto.TransactionCategoryMasterId,
                    TransactionDescription = transactionDto.TransactionDescription,
                    // TransactionDate = transactionDto.TransactionDate,
                    RecurringDay = transactionDto.RecurringDay,
                    
                    TransactionNote = transactionDto.TransactionNote,
                    TransactionPaymentmodeId =  transactionDto.TransactionPaymentModeId,
                    CreatedAt = System.DateTime.UtcNow,
                    IsActive = 1,
                    UserId = transactionDto.UserId
                };

                return  transactionRepository.AddFixedTransaction(newTransaction);
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error adding new fixed  transaction. Please try again.\"", ex); ;
            }
        }
        
        #endregion
    }
}
