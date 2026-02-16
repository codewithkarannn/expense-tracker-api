using System;
using System.Collections.Generic;

namespace Budget_Tracker_WebAPI.Models;

public partial class BudgetMaster
{
    public Guid BudgetMasterId { get; set; }

    public Guid UserId { get; set; }

    public int CategoryId { get; set; }

    public double Amount { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? TransactionCategoryMasterId { get; set; }

    public Guid? UserMasterId { get; set; }

    public virtual TransactionCategoryMaster? TransactionCategoryMaster { get; set; }

    public virtual UserMaster? UserMaster { get; set; }
}
