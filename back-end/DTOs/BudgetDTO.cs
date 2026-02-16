using AutoMapper;

namespace Budget_Tracker_WebAPI.DTOs;

public class BudgetDto
{
    public Guid BudgetMasterId { get; set; }
    public Guid UserId { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public double Amount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}
public class CreateBudgetDto
{
    public Guid UserId { get; set; }
    public int CategoryId { get; set; }
    public double Amount { get; set; }
}
public class BudgetDetailsDto
{
    public Guid BudgetMasterId { get; set; }
    public Guid UserId { get; set; }
    public int CategoryId { get; set; } 
    public string? CategoryName { get; set; }
    public double Amount { get; set; }
    public double UsedAmount { get; set; }
    public double UsedPercentage { get; set; }   
}