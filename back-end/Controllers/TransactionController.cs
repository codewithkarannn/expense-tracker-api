using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Budget_Tracker_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(ITransactionService transactionService) : ControllerBase
    {
        #region Trasactions
        
        [HttpPost("addtransaction")]
        public IActionResult AddTransaction(CreateTransactionDto transactionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var responses = new ResponseModel<object>("Validation failed");
                    return  BadRequest(responses);
                }

                var addedTransaction =  transactionService.AddTransaction(transactionDto);

                var response = new ResponseModel<TransactionDto>(addedTransaction, "Transaction added successfully", 201);
                return Ok( response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }
        
        [HttpPut("edittransaction")]
        public IActionResult EditTransaction(EditTransactionDto transactionDto)
        {
            try
            {

                var updatedTransaction =   transactionService.EditTransaction(transactionDto);

                var response = new ResponseModel<TransactionDto>(updatedTransaction, "Transaction updated successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }


        [HttpGet("usertransactions/{userId}/{page}/{pageSize}")]
        public async Task<IActionResult> GetAllTransactionsByUserId(Guid userId,  int page = 1 , int pageSize =  10)
        {
            try
            {
                var transactions = await transactionService.GetAllTransactionsByUserId(userId , page , pageSize);

                if (transactions.TransactionList.Count == 0)
                {
                    var errResponse = new ResponseModel<object>("No transactions found", StatusCodes.Status404NotFound);
                    return NotFound(errResponse);
                }

                var response = new ResponseModel<PaginatedTransactionDto>(transactions, "Transactions retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        
        [HttpGet("paginatedtransactions/{userId}")]
        public async Task<IActionResult> GetAllTransactionsByUserId(Guid userId,[FromQuery] TransactionQueryParameters queryParams) 
        {
            try
            {
               
                var (pagedTransactions, totalItems) = await transactionService.GetPaginatedTransactionsByUserId( userId, queryParams);

                if ( !pagedTransactions.Any())
                {
                    var emptyResponse = new PaginatedResponseModel<TransactionDto>(new List<TransactionDto>(), queryParams.Page, queryParams.PageSize, 0);
                    return Ok(emptyResponse);
                }

                var response = new PaginatedResponseModel<TransactionDto>(pagedTransactions, queryParams.Page, queryParams.PageSize, totalItems);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 500); 
                return StatusCode(response.StatusCode, response);
            }
        }


        [HttpGet("recent-transactions/{userId}")]
        public async Task<IActionResult> GetRecentTransactionsByUserId(Guid userId)
        {
            try
            {
                var transactions = await transactionService.GetRecentTransactionsByUserId(userId);

                if ( transactions.Count == 0)
                {
                    var errResponse = new ResponseModel<object>("No transactions found", StatusCodes.Status404NotFound);
                    return NotFound(errResponse);
                }

                var response = new ResponseModel<List<TransactionDto>>(transactions, "Transactions retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpDelete("deletetransaction/{transactionMasterId}")]
        public IActionResult DeleteTransaction(Guid transactionMasterId)
        {
            try
            {
                transactionService.DeleteTransaction(transactionMasterId);

                var response = new ResponseModel<object?>(null, "Transaction deleted successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

         [HttpGet("transactionsummary")]
        public async Task<IActionResult> GetTransactionSummary(Guid userId)
        {
            try
            {
                var transactionsummary = await transactionService.GetTransactionSummary(userId);

                var response = new ResponseModel<TransactionTotalDto>(transactionsummary, "Transactions summary   retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }
        
        [HttpGet("transactionsummary/{userId}/{numberOfMonths?}")]
        public async Task<IActionResult> GetTransactionSummary(Guid userId , int? numberOfMonths )
        {
            try
            {
                var transactionsummary = await transactionService.GetTransactionSummary(userId , numberOfMonths);

                if (transactionsummary == null )
                {
                    var errResponse = new ResponseModel<object>("No transaction category found", StatusCodes.Status404NotFound);
                    return NotFound(errResponse);
                }

                var response = new ResponseModel<TransactionSummaryDto>(transactionsummary, "Transactions summary   retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }


        [HttpGet("transactionscount/{userId}")]
        public async Task<IActionResult> GetCountOfTransactions(Guid userId , [FromQuery] int numberOfMonths)
        {
            try
            {
                var transactionCount = await transactionService.GetCountOfTransactions(userId, numberOfMonths);

                if (transactionCount == 0)
                {
                    var errResponse = new ResponseModel<object>("No transactions found", StatusCodes.Status404NotFound);
                    return NotFound(errResponse);
                }

                var response = new ResponseModel<int>(transactionCount, "Transactions Count retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }
       
        [HttpGet("{transactionMasterId}")]
        public  IActionResult GetTransactionByTransactionId(Guid transactionMasterId)
        {
            try
            {
                var transaction =  transactionService.GetTransactionByTransactionId(transactionMasterId);

                var response = new ResponseModel<TransactionDto>(transaction, "Transaction retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

         
        [HttpGet("expensepiechartdata/{userId}/{numberOfMonths}")]
        public async Task<IActionResult> GetExpensePieChartData(Guid userId, int  numberOfMonths = 1)
        {
            try
            {
                var transactions = await transactionService.GetExpensePieChartData(userId , numberOfMonths );

                if (transactions == null || transactions.Count == 0)
                {
                    var errResponse = new ResponseModel<object>("No transactions found", StatusCodes.Status404NotFound);
                    return NotFound(errResponse);
                }

                var response = new ResponseModel<List<ExpensePieChartDto>>(transactions, "Transactions retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }
        
        [HttpGet("monthlyexpenselinechartdata/{userId}/{numberOfMonths}")]
        public async Task<IActionResult> GetMonthlyExpenseLineChartData(Guid userId, int  numberOfMonths = 1)
        {
            try
            {
                var transactions = await transactionService.GetMonthlyExpenseLineChartData(userId , numberOfMonths );

                if (transactions == null )
                {
                    var errResponse = new ResponseModel<object>("No transactions found", StatusCodes.Status404NotFound);
                    return NotFound(errResponse);
                }

                var response = new ResponseModel<MonthlyExpenseOverviewDto>(transactions, "Transactions retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }
        
      
        [HttpGet("currencies")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            try
            {
                var currencies = await transactionService.GetAllCurrencies();

                if (currencies == null)
                {
                    var errResponse = new ResponseModel<object>("No transactions found", StatusCodes.Status404NotFound);
                    return NotFound(errResponse);
                }
              
                var response = new ResponseModel<List<CurrencyMasterDto>>(currencies, "Transactions Count retrieved successfully");
                return Ok(response);
                
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }
        #endregion
      
        #region TransactionType
         [HttpPost("addtransactiontype")]
        public async Task<IActionResult> AddTransactionType(AddTransactionTypeMasterDto modelDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var responses = new ResponseModel<object>("Validation failed");
                    return BadRequest(responses);
                }

                var addedTransactionType = await transactionService.AddTransactionType(modelDto);

                var response = new ResponseModel<TransactionTypeMasterDto>(addedTransactionType, "Transaction type added successfully", 201);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }


        [HttpGet("transactionTypes/{userMasterId}")]
        public async Task<IActionResult> GetAllTransactionTypes(Guid? userMasterId)
        {
            try
            {
                var transactions = await transactionService.GetAllTransactionTypes(userMasterId);

                if (transactions.Count == 0)
                {
                    var errResponse = new ResponseModel<object>("No transaction category found", StatusCodes.Status404NotFound);
                    return NotFound(errResponse);
                }

                var response = new ResponseModel<List<TransactionTypeMasterDto>>(transactions, "Transactions  types  retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }


        [HttpDelete("deletetransactiontype")]
        public IActionResult DeleteTransactionType(int transactionTypeMasterId)
        {
            try
            {
                transactionService.DeleteTransactionType(transactionTypeMasterId);

                var response = new ResponseModel<object?>(null, "Transaction type deleted successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }
        
        
        #endregion

        #region TransactionCategory
        [HttpPost("addtransactioncategory")]
        public async Task<IActionResult> AddTransactionCategory(AddTransactionCategoryMasterDto modelDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var responses = new ResponseModel<object>("Validation failed");
                    return BadRequest(responses);
                }

                var addedTransactionCategrory = await transactionService.AddTransactionCategory(modelDto);

                var response = new ResponseModel<TransactionCategoryMasterDto>(addedTransactionCategrory, "Transaction category added successfully", 201);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpDelete("deletetransactioncategory")]
        public IActionResult DeleteTransactionCategory(int transactionCategoryMasterId)
        {
            try
            {
                transactionService.DeleteTransactionCategory(transactionCategoryMasterId);

                var response = new ResponseModel<object?>(null, "Transaction type deleted successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        
        [HttpGet("transactioncategories/{userMasterId}/{transactionTypeMasterId}")]
        public async Task<IActionResult> GetAllTransactionCategories(Guid userMasterId ,  int transactionTypeMasterId)
        {
            try
            {
                var transactions = await transactionService.GetAllTransactionCategories(userMasterId , transactionTypeMasterId);

                if (transactions.Count == 0)
                {
                    var errResponse = new ResponseModel<object>("No transaction category found", StatusCodes.Status404NotFound);
                    return NotFound(errResponse);
                }

                var response = new ResponseModel<List<TransactionCategoryMasterDto>>(transactions, "Transactions  category retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        

        #endregion
        
        #region TransactionPaymentMode
        
        [HttpPost("addtransactionpaymentmode")]
        public async Task<IActionResult> AddTransactionPaymentMode(TransactionPaymentModeDto modelDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var responses = new ResponseModel<object>("Validation failed");
                    return BadRequest(responses);
                }

                var addedTransactionPaymentMode = await transactionService.AddTransactionPayementMode(modelDto);

                var response = new ResponseModel<TransactionPaymentModeDto>(addedTransactionPaymentMode, "Transaction payment mode added successfully", 201);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpDelete("deletetransactionpaymentmode/{transactionCategoryMasterId}")]
        public IActionResult DeleteTransactionPaymentMode(int transactionCategoryMasterId)
        {
            try
            {
                transactionService.DeleteTransactionPayementMode(transactionCategoryMasterId);

                var response = new ResponseModel<object?>(null, "Transaction payment mode deleted successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }
        
        [HttpGet("transactionPaymentModes/{userMasterId}")]
        public async Task<IActionResult> GetAllTransactionPaymentMode(Guid? userMasterId)
        {
            try
            {
                var paymentModes = await transactionService.GetAllPaymentMode(userMasterId);

                if (paymentModes.Count == 0)
                {
                    var errResponse = new ResponseModel<object>("No transaction payment mode found", StatusCodes.Status404NotFound);
                    return NotFound(errResponse);
                }

                var response = new ResponseModel<List<TransactionPaymentModeDto>>(paymentModes, "Transactions  payment modes  retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }


        #endregion
        
        
        #region  Fixed Transaction
        
        [HttpPost("addfixedransaction")]
        public ActionResult<ResponseModel<FixedTransactionDto>> AddFixedTransaction(CreateFixedTransactionDto transactionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var responses = new ResponseModel<object>("Validation failed");
                    return  BadRequest(responses);
                }

                var addedTransaction =  transactionService.AddFixedTransaction(transactionDto);

                var response = new ResponseModel<FixedTransactionDto>(addedTransaction, "Fixed Transaction added successfully", 201);
                return Ok( response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message);
                return StatusCode(response.StatusCode, response);
            }
        }

        #endregion
    }
}