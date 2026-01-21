using System;
using System.Collections.Generic;

namespace Budget_Tracker_WebAPI.Models;

public partial class UserMaster
{
    public Guid UserMasterId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? UserRoleId { get; set; }
    
    public int? CurrencyMasterId { get; set; }

    public string? UserEmail { get; set; }

    public string? UserPassword { get; set; }
    

    public sbyte? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<TransactionCategoryMaster> TransactionCategoryMasters { get; set; } = new List<TransactionCategoryMaster>();

    public virtual ICollection<TransactionMaster> TransactionMasters { get; set; } = new List<TransactionMaster>();

    public virtual ICollection<TransactionPaymentMode> TransactionPaymentModes { get; set; } = new List<TransactionPaymentMode>();

    public virtual ICollection<TransactionTypeMaster> TransactionTypeMasters { get; set; } = new List<TransactionTypeMaster>();
    
    public virtual ICollection<BudgetMaster> BudgetMasters { get; set; } = new List<BudgetMaster>();


    public virtual UserRoleMaster? UserRole { get; set; }
    
    public virtual CurrencyMaster? CurrencyMaster { get; set; }
}
