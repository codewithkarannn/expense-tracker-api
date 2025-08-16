using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;
using Budget_Tracker_WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budget_Tracker_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            this._transactionService = transactionService;
        }

        [HttpPost("addtransaction")]
        public async Task<IActionResult> AddTransaction(TransactionDTO transactionDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var responses = new ResponseModel<object>("Validation failed", StatusCodes.Status400BadRequest);
                    return BadRequest(responses);
                }

                var addedTransaction = await _transactionService.AddTransaction(transactionDTO);

                var response = new ResponseModel<TransactionDTO>(addedTransaction, "Transaction added successfully", 201);
                return Ok( response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpPost("addtransactiontype")]
        public async Task<IActionResult> AddTransactionType(AddTransactionTypeMasterDTO modelDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var responses = new ResponseModel<object>("Validation failed", StatusCodes.Status400BadRequest);
                    return BadRequest(responses);
                }

                var addedTransactionType = await _transactionService.AddTransactionType(modelDTO);

                var response = new ResponseModel<TransactionTypeMasterDTO>(addedTransactionType, "Transaction type added successfully", 201);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }



        [HttpPost("addtransactioncategory")]
        public async Task<IActionResult> AddTransactionCategory(AddTransactionCategoryMasterDTO modelDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var responses = new ResponseModel<object>("Validation failed", StatusCodes.Status400BadRequest);
                    return BadRequest(responses);
                }

                var addedTransactionCategrory = await _transactionService.AddTransactionCategory(modelDTO);

                var response = new ResponseModel<TransactionCategoryMasterDTO>(addedTransactionCategrory, "Transaction category added successfully", 201);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }


        [HttpPost("addtransactionpaymentmode")]
        public async Task<IActionResult> AddTransactionPaymentMode(TransactionPaymentModeDTO modelDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var responses = new ResponseModel<object>("Validation failed", StatusCodes.Status400BadRequest);
                    return BadRequest(responses);
                }

                var addedTransactionPaymentMode = await _transactionService.AddTransactionPayementMode(modelDTO);

                var response = new ResponseModel<TransactionPaymentModeDTO>(addedTransactionPaymentMode, "Transaction payment mode added successfully", 201);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpPut("edittransaction")]
        public async Task<IActionResult> EditTransaction(EditTransactionDTO transactionDTO)
        {
            try
            {

                var updatedTransaction =  await _transactionService.EditTransaction(transactionDTO);

                var response = new ResponseModel<TransactionDTO>(updatedTransaction, "Transaction updated successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("transactions/{userID}")]
        public async Task<IActionResult> GetAllTransactionsByUserID(Guid userID,  int page = 1 , int pageSize =  10)
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionsByUserID(userID , page , pageSize);

                if (transactions == null || transactions.Count == 0)
                {
                    var err_response = new ResponseModel<object>("No transactions found", StatusCodes.Status404NotFound);
                    return NotFound(err_response);
                }

                var response = new ResponseModel<List<TransactionDTO>>(transactions, "Transactions retrieved successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("paginatedtransactions/{userID}")]
        public async Task<IActionResult> GetAllTransactionsByUserID(Guid userID,[FromQuery] TransactionQueryParameters queryParams) // The only two parameters!
        {
            try
            {
                // Your service method will now also be cleaner.
                // It should return both the paged data and the total count of items matching the filters.
                var (pagedTransactions, totalItems) = await _transactionService.GetPaginatedTransactionsByUserID(
                    userID, queryParams // Pass the entire object to the service layer
                );

                if (pagedTransactions == null || !pagedTransactions.Any())
                {
                    var emptyResponse = new PaginatedResponseModel<TransactionDTO>(new List<TransactionDTO>(), queryParams.Page, queryParams.PageSize, 0);
                    return Ok(emptyResponse);
                }

                var response = new PaginatedResponseModel<TransactionDTO>(pagedTransactions, queryParams.Page, queryParams.PageSize, totalItems);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 500); // Use 500 for server errors
                return StatusCode(response.StatusCode, response);
            }
        }


        [HttpGet("recent-transactions/{userID}")]
        public async Task<IActionResult> GetRecentTransactionsByUserID(Guid userID)
        {
            try
            {
                var transactions = await _transactionService.GetRecentTransactionsByUserID(userID);

                if (transactions == null || transactions.Count == 0)
                {
                    var err_response = new ResponseModel<object>("No transactions found", StatusCodes.Status404NotFound);
                    return NotFound(err_response);
                }

                var response = new ResponseModel<List<TransactionDTO>>(transactions, "Transactions retrieved successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpDelete("deletetransaction/{transactionMasterId}")]
        public IActionResult DeleteTransaction(Guid transactionMasterId)
        {
            try
            {
                _transactionService.DeleteTransaction(transactionMasterId);

                var response = new ResponseModel<object>(null, "Transaction deleted successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpDelete("deletetransactiontype")]
        public IActionResult DeleteTransactionType(int transactionTypeMasterId)
        {
            try
            {
                _transactionService.DeleteTransactionType(transactionTypeMasterId);

                var response = new ResponseModel<object>(null, "Transaction type deleted successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpDelete("deletetransactioncategory")]
        public IActionResult DeleteTransactionCategory(int transactionCategoryMasterId)
        {
            try
            {
                _transactionService.DeleteTransactionCategory(transactionCategoryMasterId);

                var response = new ResponseModel<object>(null, "Transaction type deleted successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }


        [HttpDelete("deletetransactionpaymentmode")]
        public IActionResult DeleteTransactionPaymentMode(int transactionCategoryMasterId)
        {
            try
            {
                _transactionService.DeleteTransactionPayementMode(transactionCategoryMasterId);

                var response = new ResponseModel<object>(null, "Transaction payment mode deleted successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("{transactionMasterId}")]
        public  IActionResult GetTransactionByTransactionID(Guid transactionMasterId)
        {
            try
            {
                var transaction =  _transactionService.GetTransactionByTransactionID(transactionMasterId);

                if (transaction == null)
                {
                    var err_response = new ResponseModel<object>("Transaction not found", StatusCodes.Status404NotFound);
                    return NotFound(err_response);
                }

                var response = new ResponseModel<TransactionDTO>(transaction, "Transaction retrieved successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }


        [HttpGet("transactioncategories")]
        public async Task<IActionResult> GetAllTransactionCategories(Guid userMasterID)
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionCategories(userMasterID);

                if (transactions == null || transactions.Count == 0)
                {
                    var err_response = new ResponseModel<object>("No transaction category found", StatusCodes.Status404NotFound);
                    return NotFound(err_response);
                }

                var response = new ResponseModel<List<TransactionCategoryMasterDTO>>(transactions, "Transactions  category retrieved successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }


        [HttpGet("transactionTypes")]
        public async Task<IActionResult> GetAllTransactionTypes(Guid? userMasterID)
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionTypes(userMasterID);

                if (transactions == null || transactions.Count == 0)
                {
                    var err_response = new ResponseModel<object>("No transaction category found", StatusCodes.Status404NotFound);
                    return NotFound(err_response);
                }

                var response = new ResponseModel<List<TransactionTypeMasterDTO>>(transactions, "Transactions  types  retrieved successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("transactionPaymentModes")]
        public async Task<IActionResult> GetAllTransactionPaymentMode(Guid? userMasterID)
        {
            try
            {
                var paymentModes = await _transactionService.GetAllPaymentMode(userMasterID);

                if (paymentModes == null || paymentModes.Count == 0)
                {
                    var err_response = new ResponseModel<object>("No transaction payment mode found", StatusCodes.Status404NotFound);
                    return NotFound(err_response);
                }

                var response = new ResponseModel<List<TransactionPaymentModeDTO>>(paymentModes, "Transactions  payment modes  retrieved successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("transactionsummary")]
        public async Task<IActionResult> GetTransactionSummary(Guid userId)
        {
            try
            {
                var transactionsummary = await _transactionService.GetTransactionSummary(userId);

                if (transactionsummary == null )
                {
                    var err_response = new ResponseModel<object>("No transaction category found", StatusCodes.Status404NotFound);
                    return NotFound(err_response);
                }

                var response = new ResponseModel<TransactionTotalDTO>(transactionsummary, "Transactions summary   retrieved successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }


        [HttpGet("transactionscount/{userID}/{numberofmonths}")]
        public async Task<IActionResult> GetCountOfTransactions(Guid userID , int numberOfMonths)
        {
            try
            {
                var transactionCount = await _transactionService.GetCountOfTransactions(userID , numberOfMonths);

                if (transactionCount == null)
                {
                    var err_response = new ResponseModel<object>("No transactions found", StatusCodes.Status404NotFound);
                    return NotFound(err_response);
                }

                var response = new ResponseModel<int>(transactionCount, "Transactions Count retrieved successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }
    }
}