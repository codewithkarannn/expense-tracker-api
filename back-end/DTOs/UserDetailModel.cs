namespace Budget_Tracker_WebAPI.DTOs
{
    public class UserDetailModel
    {
        public Guid UserMasterId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int? UserRoleId { get; set; }

        public string? UserEmail { get; set; }
        public string? UserRole { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? CurrencyCode { get; set; }
        public int? CurrencyMasterId { get; set; }

    }
}
