using System;
using System.Collections.Generic;

namespace Budget_Tracker_WebAPI.Models;

public partial class TransactionCategoryMaster
{
    public int TransactionCategoryMasterId { get; set; }

    public string TransactionCategoryName { get; set; } = null!;

    public sbyte IsActive { get; set; }

    public Guid? UserMasterId { get; set; }

    public int? TransactionTypeMasterId { get; set; }

    public virtual ICollection<BudgetMaster> BudgetMasters { get; set; } = new List<BudgetMaster>();

    public virtual ICollection<FixedTransactionMaster> FixedTransactionMasters { get; set; } = new List<FixedTransactionMaster>();

    public virtual ICollection<TransactionMaster> TransactionMasters { get; set; } = new List<TransactionMaster>();

    public virtual TransactionTypeMaster? TransactionTypeMaster { get; set; }

    public virtual UserMaster? UserMaster { get; set; }
}
