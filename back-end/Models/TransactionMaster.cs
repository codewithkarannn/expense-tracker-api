using System;
using System.Collections.Generic;

namespace Budget_Tracker_WebAPI.Models;

public partial class TransactionMaster
{
    public Guid TransactionMasterId { get; set; }

    public Guid UserId { get; set; }

    public int TransactionTypeMasterId { get; set; }

    public int TransactionCategoryMasterId { get; set; }

    public int? TransactionPaymentmodeId { get; set; }

    public string TransactionDescription { get; set; } = null!;

    public double TransactionAmount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? TransactionNote { get; set; }

    public sbyte? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual TransactionCategoryMaster TransactionCategoryMaster { get; set; } = null!;

    public virtual TransactionPaymentMode? TransactionPaymentmode { get; set; }

    public virtual TransactionTypeMaster TransactionTypeMaster { get; set; } = null!;
    
    

    public virtual UserMaster User { get; set; } = null!;
}
