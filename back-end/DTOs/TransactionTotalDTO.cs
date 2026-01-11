namespace Budget_Tracker_WebAPI.DTOs
{
    public class TransactionTotalDTO
    {
        public Guid UserId { get; set; }
        public string? CurrentBalance { get; set; }
      
        public string? CurrentExpense { get; set; }
        public string? TodayIncome { get; set; }
        public string? WeekIncome { get; set; }
        public string? MonthIncome { get; set; }
        public string? DayIncomeChange { get; set; }
        public string? WeekIncomeChange { get; set; }
        public string?   MonthIncomeChange { get; set; }

        public string? TodayExpense { get; set; }
        public string? WeekExpense { get; set; }
        public string? MonthExpense { get; set; }
        public string? DayExpenseChange { get; set; }
        public string? WeekExpenseChange { get; set; }
        public string? MonthExpenseChange { get; set; }
        
        public string? CurrentIncome { get; set; }

    }
}
