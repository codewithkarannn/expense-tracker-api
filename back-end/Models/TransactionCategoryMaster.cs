using System;
using System.Collections.Generic;

namespace Budget_Tracker_WebAPI.Models;

public partial class TransactionCategoryMaster
{
    public int TransactionCategoryMasterId { get; set; }
    
    public int? TransactionTypeMasterId { get; set; }

    public string TransactionCategoryName { get; set; } = null!;

    public sbyte IsActive { get; set; }

    public Guid? UserMasterId { get; set; }

    public virtual ICollection<TransactionMaster> TransactionMasters { get; set; } = new List<TransactionMaster>();
    public virtual ICollection<BudgetMaster> BudgetMasters { get; set; } = new List<BudgetMaster>();



    public virtual UserMaster? UserMaster { get; set; }
    
    public virtual TransactionTypeMaster? TransactionTypeMaster { get; set; }
}
