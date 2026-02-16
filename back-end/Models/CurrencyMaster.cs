using System;
using System.Collections.Generic;

namespace Budget_Tracker_WebAPI.Models;

public partial class CurrencyMaster
{
    public int CurrencyMasterId { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public string CurrencyName { get; set; } = null!;

    public string CurrencySymbol { get; set; } = null!;

    public virtual ICollection<UserMaster> UserMasters { get; set; } = new List<UserMaster>();
}
