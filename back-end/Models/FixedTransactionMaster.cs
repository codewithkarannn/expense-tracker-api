using System;
using System.Collections.Generic;

namespace Budget_Tracker_WebAPI.Models;

public partial class FixedTransactionMaster
{
    public Guid FixedTransactionMasterId { get; set; }

    public Guid UserId { get; set; }

    public int TransactionTypeMasterId { get; set; }

    public int TransactionCategoryMasterId { get; set; }

    public int? TransactionPaymentmodeId { get; set; }

    public int RecurringDay { get; set; }

    public string? TransactionDescription { get; set; }

    public decimal TransactionAmount { get; set; }

    public string? TransactionNote { get; set; }

    public sbyte? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid UserMasterId { get; set; }

    public virtual TransactionCategoryMaster TransactionCategoryMaster { get; set; } = null!;

    public virtual TransactionPaymentMode? TransactionPaymentmode { get; set; }

    public virtual TransactionTypeMaster TransactionTypeMaster { get; set; } = null!;
}
