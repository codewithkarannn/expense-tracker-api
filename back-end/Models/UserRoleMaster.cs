using System;
using System.Collections.Generic;

namespace Budget_Tracker_WebAPI.Models;

public partial class UserRoleMaster
{
    public int UserRoleMasterId { get; set; }

    public string? UserRole { get; set; }

    public sbyte IsActive { get; set; }

    public virtual ICollection<UserMaster> UserMasters { get; set; } = new List<UserMaster>();
}
