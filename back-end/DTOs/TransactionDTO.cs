using System.ComponentModel.DataAnnotations;
using Budget_Tracker_WebAPI.Models;

namespace Budget_Tracker_WebAPI.DTOs
{
    public class TransactionDto
    {
        public Guid? TransactionMasterId { get; set; } = Guid.Empty;

        public Guid UserId { get; set; }

        public int TransactionTypeMasterId { get; set; }
       

        public int TransactionCategoryMasterId { get; set; }
        public int? TransactionPaymentModeId { get; set; }

        public string? TransactionPaymentMode { get; set; } = null!;

        public string TransactionDescription { get; set; } = null!;

        public double TransactionAmount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string? TransactionNote { get; set; }

        public sbyte? IsActive { get; set; }

        public string? TransactionType { get; set; }
        public string?  TransactionCategory { get; set; }
        public DateTime? CreatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

    }


    public class PaginatedTransactionDto
    {
        public int TotalRecords { get; set; } 
        public int PageSize { get; set; } 
        public int Page { get; set; }

        public List<TransactionDto> TransactionList { get; set; }
    }
    
    public class CreateTransactionDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int TransactionTypeMasterId { get; set; }

        [Required]
        public int TransactionCategoryMasterId { get; set; }

        [Required]
        public int TransactionPaymentModeId { get; set; }

        [Required]
        [MaxLength(500)]
        public string TransactionDescription { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public double TransactionAmount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [MaxLength(1000)]
        public string? TransactionNote { get; set; }
    }
     
    
    public partial class TransactionCategoryMasterDTO
    {
        public int TransactionCategoryMasterId { get; set; }

        public string TransactionCategoryName { get; set; } = null!;
        public bool? IsCustom { get; set; } = false!;


    }


    public partial class AddTransactionCategoryMasterDTO
    {
        public int? TransactionCategoryMasterId { get; set; }

        public string TransactionCategoryName { get; set; } = null!;

        public Guid UserMasterId { get; set; }

    }
    
    public partial class TransactionTypeMasterDTO
    {
        public int TransactionTypeMasterId { get; set; }

        public string? TransactionTypename { get; set; }


        public bool? IsCustom { get; set; } = false!;

    }

    public partial class AddTransactionTypeMasterDTO
    {
        public int? TransactionTypeMasterId { get; set; }
        public Guid UserMasterId { get; set; }

        public string? TransactionTypename { get; set; }




    }


 
    public class EditTransactionDTO
    {
        public Guid TransactionMasterId { get; set; }

        public Guid UserId { get; set; }

        public int TransactionTypeMasterId { get; set; }

        public int TransactionCategoryMasterId { get; set; }


        public int TransactionPaymentModeId { get; set; }

        public string TransactionPaymentMode { get; set; } = null!;

        public string TransactionDescription { get; set; } = null!;

        public double TransactionAmount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string? TransactionNote { get; set; }

        public sbyte? IsActive { get; set; }

        public string? TransactionType { get; set; }
        public string? TransactionCategory { get; set; }
        public DateTime? CreatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

    }


    public  class TransactionPaymentModeDTO
    {
        public int? PaymentModeId { get; set; }

        public string PaymentMode { get; set; } = null!;

        public sbyte IsActive { get; set; }

        public Guid? UserMasterId { get; set; }

        public bool IsCustom { get; set; }
    }


    public class ExpensePieChartDTO
    {
        public string ExpenseCategory { get; set; }

        public decimal Amount { get; set; }
    }

    public class MonthlyExpenseOverviewDTO
    {
        public decimal TotalMonthlyExpense { get; set; }
        public decimal PercentageLastMonthlyExpense { get; set; }
        public string Sign { get; set; }
        
        public List<MonthlyExpenseDTO> MonthlyExpenses { get; set; }
    }

    public class MonthlyExpenseDTO
    {
        public decimal Amount { get; set; }
        public string Month { get; set; }
        public int MonthNumber { get; set; }
        public string Year { get; set; }
    }
    
    
    public class CurrencyMasterDTO
    {
        public int CurrencyMasterId { get; set; }
    
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
  
       
    }
}
