using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;
using Budget_Tracker_WebAPI.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Budget_Tracker_WebAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public TransactionDTO AddTransaction(TransactionDTO transactionDTO)
        {
            try
            {

                var newTransaction = new TransactionMaster
                {
                    TransactionTypeMasterId = transactionDTO.TransactionTypeMasterId,
                    TransactionAmount = transactionDTO.TransactionAmount,
                    TransactionCategoryMasterId = transactionDTO.TransactionCategoryMasterId,
                    TransactionDescription = transactionDTO.TransactionDescription,
                    TransactionDate = transactionDTO.TransactionDate,
                    TransactionNote = transactionDTO.TransactionNote,
                    TransactionPaymentmodeId =  transactionDTO.TransactionPaymentModeId,
                    CreatedAt = System.DateTime.UtcNow,
                    IsActive = 1,
                    UserId = transactionDTO.UserId
                };

                return  _transactionRepository.AddTransaction(newTransaction);
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error adding new  transaction. Please try again.\"", ex); ;
            }
        }

        public async Task<TransactionTypeMasterDTO> AddTransactionType(AddTransactionTypeMasterDTO modelDTO)
        {   
            try
            {       
                var IsPresent = await  _transactionRepository.IsTransactionTypePresent(modelDTO.UserMasterId, modelDTO?.TransactionTypename);

                if(IsPresent != true)
                {

                    var newModel = new TransactionTypeMaster
                    {
                        
                        TransactionTypename = modelDTO.TransactionTypename,
                        IsActive = 1,
                        UserMasterId = modelDTO.UserMasterId
                    };

                    return  _transactionRepository.AddTransactionType(newModel);
                }
                throw new Exception("Transaction type already present");
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error adding new  transaction. Please try again.\"", ex); ;
            }
        }

        public async Task<TransactionCategoryMasterDTO> AddTransactionCategory(AddTransactionCategoryMasterDTO modelDTO)
        {
            try
            {
                var IsPresent = await _transactionRepository.IsTransactionCategoryPresent(modelDTO.UserMasterId, modelDTO?.TransactionCategoryName);

                if (IsPresent != true)
                {

                    var newModel = new TransactionCategoryMaster
                    {
                        
                        TransactionCategoryName = modelDTO.TransactionCategoryName,
                        IsActive = 1,
                        UserMasterId = modelDTO.UserMasterId
                    };

                    return  _transactionRepository.AddTransactionCategory(newModel);
                }
                throw new Exception("Transaction category already present");
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error adding new  transaction. Please try again.\"", ex); ;
            }
        }

        public async Task<TransactionPaymentModeDTO> AddTransactionPayementMode(TransactionPaymentModeDTO modelDTO)
        {
            try
            {

                var IsPresent = await _transactionRepository.IsTransactionPaymentModePresent(modelDTO?.UserMasterId ?? null, modelDTO?.PaymentMode);

                if (IsPresent != true)
                {

                    var newModel = new TransactionPaymentMode
                    {

                        
                        IsActive = 1,
                        PaymentMode = modelDTO.PaymentMode,
                        
                        UserMasterId = modelDTO.UserMasterId
                    };

                    return  _transactionRepository.AddPaymentMode(newModel);
                }
                throw new Exception("Transaction payment mode already present");
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error adding new  payment mode. Please try again.\"", ex); ;
            }
        }

        public TransactionDTO EditTransaction(EditTransactionDTO transactionDTO)
        {
            try
            {
                if(transactionDTO.TransactionMasterId  != Guid.Empty)
                {
                    var existingTransaction =   _transactionRepository.GetTransactionByTransactionMasterID(transactionDTO.TransactionMasterId);

                    if (existingTransaction != null)
                    {
                        var newTransaction = new TransactionMaster
                        {
                            TransactionTypeMasterId = transactionDTO.TransactionTypeMasterId,
                            TransactionAmount = transactionDTO.TransactionAmount,
                            TransactionCategoryMasterId = transactionDTO.TransactionCategoryMasterId,
                            TransactionDescription = transactionDTO.TransactionDescription,
                            TransactionDate = System.DateTime.UtcNow,
                            TransactionPaymentmodeId = transactionDTO.TransactionPaymentModeId,
                            TransactionNote = transactionDTO.TransactionNote,
                            TransactionMasterId = transactionDTO.TransactionMasterId,

                            UserId = transactionDTO.UserId
                        };

                        return _transactionRepository.EditTransaction(newTransaction);

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

        public async Task<List<TransactionDTO>> GetAllTransactionsByUserID(Guid userID, int page, int pageSize)
        {
            try
            {

                var transactions = await _transactionRepository.GetAllTransactionByUserID(userID,  page,  pageSize);
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


        public async Task<List<TransactionDTO>> GetRecentTransactionsByUserID(Guid userID)
        {
            try
            {

                var transactions = await _transactionRepository.GetRecentTransactionsByUserID(userID );
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
                var existingTransaction =    _transactionRepository.GetTransactionByTransactionMasterID(transactionMasterID);

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

                     _transactionRepository.Delete(selectedTransaction);

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
                var existingTransaction =  _transactionRepository.GetTransactionTypeByTransactionTypeMasterID(transactionTypeMasterID);

                if (existingTransaction != null)
                {
                    var selectedTransaction = new TransactionTypeMaster
                    {
                        TransactionTypeMasterId = existingTransaction.TransactionTypeMasterId,
                        TransactionTypename = existingTransaction.TransactionTypename,
                        IsActive = 0,
                        UserMasterId = existingTransaction.UserMasterId,
                    };

                    _transactionRepository.DeleteTransactionType(selectedTransaction);

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
                var existingTransaction =  _transactionRepository.GetTransactionCategoryByTransactionCategoryMasterID(transactionCategoryMasterID);

                if (existingTransaction != null)
                {
                    var selectedTransaction = new TransactionCategoryMaster
                    {
                        TransactionCategoryMasterId = existingTransaction.TransactionCategoryMasterId,
                        TransactionCategoryName = existingTransaction.TransactionCategoryName,

                        IsActive = 0,
                        UserMasterId = existingTransaction.UserMasterId,
                    };

                    _transactionRepository.DeleteTransactionCategory(selectedTransaction);

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
                var existingModel = _transactionRepository.GetTransactionPaymentModeByTransactionPayementModeMasterID(paymentModeMasterId);

                if (existingModel != null)
                {
                    var selectedModel = new TransactionPaymentMode
                    {
                        PaymentModeId = existingModel.PaymentModeId,
                        PaymentMode= existingModel.PaymentMode,

                        IsActive = 0,
                        UserMasterId = existingModel.UserMasterId,
                    };

                    _transactionRepository.DeletePaymentMode(selectedModel);

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


        public TransactionDTO GetTransactionByTransactionID(Guid transactionMasterID)
        {
            try
            {

                var transaction = _transactionRepository.GetTransactionByTransactionMasterID(transactionMasterID);
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

        public async Task<List<TransactionTypeMasterDTO>> GetAllTransactionTypes(Guid? userMasterID)
        {
            try
            {

                var transactionTypes = await _transactionRepository.GetAllTransactionTypes(userMasterID);

              

                if (transactionTypes != null)
                {
                    var transactionTypesDto = transactionTypes.Select(i => new TransactionTypeMasterDTO
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

        public async Task<List<TransactionCategoryMasterDTO>> GetAllTransactionCategories(Guid userMasterID)
        {
            try
            {

                var transactionCat = await _transactionRepository.GetAllTransactionCategories(userMasterID);
                if (transactionCat != null)
                {
                    var transactionCategoryDTO = transactionCat.Select(i => new TransactionCategoryMasterDTO
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

        public async Task<List<TransactionPaymentModeDTO>> GetAllPaymentMode(Guid? userMasterID)
        {
            try
            {

                var paymentModes = await _transactionRepository.GetAllTransactionPaymentMode(userMasterID);
                if (paymentModes != null)
                {
                    var paymentModeList = paymentModes.Select(i => new TransactionPaymentModeDTO
                    {
                        PaymentModeId = i.PaymentModeId,
                        UserMasterId = i.UserMasterId,
                        IsCustom = i.IsCustom,
                        IsActive = i.IsActive,
                        PaymentMode = i.PaymentMode,
                        

                    }).ToList();
                    return paymentModeList;
                }
                else
                {
                    throw new KeyNotFoundException("No transaction payment mode  found ");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching   transaction payment mode. Please try again.\"", ex); ;
            }
        }


        public async Task<TransactionTotalDTO> GetTransactionSummary(Guid userId)
        {
            try
            {

                var transaction = await _transactionRepository.GetTransactionSummary(userId);
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

                var transactionCount = await _transactionRepository.GetCountOfTransactions(userId, numberOfMonths);
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

        public async Task<(List<TransactionDTO> Transactions, int TotalItems)> GetPaginatedTransactionsByUserID(Guid userId, TransactionQueryParameters queryParams)
        {
            try
            {
                var transactions = await _transactionRepository.GetPaginatedTransactionsByUserID(userId, queryParams);

                return transactions;
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching   transaction . Please try again.\"", ex); ;
            }
        }
    }
}
