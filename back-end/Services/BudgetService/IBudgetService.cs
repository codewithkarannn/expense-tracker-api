using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;

namespace Budget_Tracker_WebAPI.Services.BudgetService;

public interface IBudgetService
{
    public Task<BudgetDto> AddNewBudget(CreateBudgetDto createBudgetDto);

    public Task<List<BudgetDetailsDto>?> GetAllBudgets(Guid userId);

    public Task<bool> DeleteBudget(Guid budgetMasterId);

}