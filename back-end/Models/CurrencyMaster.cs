namespace Budget_Tracker_WebAPI.Models;

public class CurrencyMaster
{
    public int CurrencyMasterId { get; set; }
    
    public string CurrencyCode { get; set; }
    public string CurrencyName { get; set; }
    public string CurrencySymbol { get; set; }
  
    public virtual ICollection<UserMaster> UserMasters { get; set; } = new List<UserMaster>();
}