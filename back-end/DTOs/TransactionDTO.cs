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
     
    
    public partial class TransactionCategoryMasterDto
    {
        public int TransactionCategoryMasterId { get; set; }

        public string TransactionCategoryName { get; set; } = null!;
        public bool? IsCustom { get; set; } = false!;


    }


    public partial class AddTransactionCategoryMasterDto
    {
        public int? TransactionCategoryMasterId { get; set; }

        public string TransactionCategoryName { get; set; } = null!;

        public Guid UserMasterId { get; set; }

    }
    
    public partial class TransactionTypeMasterDto
    {
        public int TransactionTypeMasterId { get; set; }

        public string? TransactionTypename { get; set; }


        public bool? IsCustom { get; set; } = false!;

    }

    public partial class AddTransactionTypeMasterDto
    {
        public int? TransactionTypeMasterId { get; set; }
        public Guid UserMasterId { get; set; }

        public string? TransactionTypename { get; set; }




    }


 
    public class EditTransactionDto
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


    public  class TransactionPaymentModeDto
    {
        public int? PaymentModeId { get; set; }

        public string PaymentMode { get; set; } = null!;

        public sbyte IsActive { get; set; }

        public Guid? UserMasterId { get; set; }

        public bool IsCustom { get; set; }
    }


    public class ExpensePieChartDto
    {
        public string ExpenseCategory { get; set; }

        public decimal Amount { get; set; }
    }

    public class MonthlyExpenseOverviewDto
    {
        public decimal TotalMonthlyExpense { get; set; }
        public decimal PercentageLastMonthlyExpense { get; set; }
        public string Sign { get; set; }
        
        public List<MonthlyExpenseDto> MonthlyExpenses { get; set; }
    }

    public class MonthlyExpenseDto
    {
        public decimal Amount { get; set; }
        public string Month { get; set; }
        public int MonthNumber { get; set; }
        public string Year { get; set; }
    }
    
    
    public class CurrencyMasterDto
    {
        public int CurrencyMasterId { get; set; }
    
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
  
       
    }
    
     public class FixedTransactionDto
    {
        public Guid? TransactionMasterId { get; set; } = Guid.Empty;

        public Guid UserId { get; set; }

        public int TransactionTypeMasterId { get; set; }
        
        public int TransactionCategoryMasterId { get; set; }

        public int? TransactionPaymentModeId { get; set; }

        public string? TransactionPaymentMode { get; set; } = null!;

        public string? TransactionDescription { get; set; } = null!;

        public double TransactionAmount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string? TransactionNote { get; set; }

        public sbyte? IsActive { get; set; }

        public string? TransactionType { get; set; }
        
        public string?  TransactionCategory { get; set; }
        
        public DateTime? CreatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
        
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public int RecurringDay { get; set; }


    }
     
    public class CreateFixedTransactionDto
    {
        public Guid? TransactionMasterId { get; set; } = Guid.Empty;

        public Guid UserId { get; set; }

        public int TransactionTypeMasterId { get; set; }
        
        public int TransactionCategoryMasterId { get; set; }

        public int? TransactionPaymentModeId { get; set; }
        
        public string? TransactionDescription { get; set; } = null!;

        public double TransactionAmount { get; set; }
        
        public string? TransactionNote { get; set; }
        
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        
        public int RecurringDay { get; set; }

    }



}
