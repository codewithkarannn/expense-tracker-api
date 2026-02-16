using System;
using System.Collections.Generic;

namespace Budget_Tracker_WebAPI.Models;

public partial class TransactionTypeMaster
{
    public int TransactionTypeMasterId { get; set; }

    public string? TransactionTypename { get; set; }

    public sbyte? IsActive { get; set; }

    public Guid? UserMasterId { get; set; }

    public virtual ICollection<FixedTransactionMaster> FixedTransactionMasters { get; set; } = new List<FixedTransactionMaster>();

    public virtual ICollection<TransactionCategoryMaster> TransactionCategoryMasters { get; set; } = new List<TransactionCategoryMaster>();

    public virtual ICollection<TransactionMaster> TransactionMasters { get; set; } = new List<TransactionMaster>();

    public virtual UserMaster? UserMaster { get; set; }
}
