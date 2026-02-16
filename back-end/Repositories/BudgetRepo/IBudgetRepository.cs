using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;

namespace Budget_Tracker_WebAPI.Repositories.BudgetRepo;

public interface IBudgetRepository
{
    public Task<BudgetDto> AddNewBudget(BudgetMaster createBudgetDto);

    public Task<BudgetDto?> GetBudget(Guid userId, int categoryId);

    public Task<List<BudgetDetailsDto>?> GetAllBudgets(Guid userId);
    public Task<bool> DeleteBudget(Guid budgetMasterId);
}