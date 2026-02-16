using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;
using Budget_Tracker_WebAPI.Repositories.BudgetRepo;

namespace Budget_Tracker_WebAPI.Services.BudgetService;

public class BudgetService (IBudgetRepository budgetRepository) : IBudgetService
{

    public async Task<BudgetDto> AddNewBudget(CreateBudgetDto createBudgetDto)
    {
        try
        {
            var existingModel = await budgetRepository.GetBudget(createBudgetDto.UserId, createBudgetDto.CategoryId);
            if (existingModel != null)
            {
                throw new InvalidOperationException("Budget already exists for this category.");
            }
                
            var model = new BudgetMaster
            {
                UserId =  createBudgetDto.UserId,
                CategoryId = createBudgetDto.CategoryId,    
                Amount = createBudgetDto.Amount,
                CreatedAt = DateTime.UtcNow,
                IsActive =  true,
                ModifiedAt = null,
                DeletedAt =  null
                
            };
            
            return  await budgetRepository.AddNewBudget(model);
        }
        catch (Exception e)
        {
            throw new Exception("\"There was an error data . Please try again.\"", e);
        }
    }

    public async Task<List<BudgetDetailsDto>?> GetAllBudgets(Guid userId)
    {
        try
        {
            return  await budgetRepository.GetAllBudgets(userId);

        }
        catch (Exception e)
        {
            throw new Exception("\"There was an error . Please try again.\"", e);
        }
    }
    
    public async Task<bool> DeleteBudget(Guid  budgetMasterId)
    {
        try
        {
            return  await budgetRepository.DeleteBudget(budgetMasterId);

        }
        catch (Exception e)
        {
            throw new Exception("\"There was an error . Please try again.\"", e);
        }
    }
}