using System;
using System.Collections.Generic;

namespace Budget_Tracker_WebAPI.Models;

public partial class TransactionPaymentMode
{
    public int PaymentModeId { get; set; }

    public string PaymentMode { get; set; } = null!;

    public sbyte IsActive { get; set; }

    public Guid? UserMasterId { get; set; }

    public virtual ICollection<TransactionMaster> TransactionMasters { get; set; } = new List<TransactionMaster>();

    public virtual UserMaster? UserMaster { get; set; }
}
