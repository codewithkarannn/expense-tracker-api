using Budget_Tracker_WebAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Budget_Tracker_WebAPI.Repositories.BudgetRepo;
using Budget_Tracker_WebAPI.Models;

public class BudgetRepository(DbExpenseTrackerContext dbContext) :IBudgetRepository
{
    
    public async Task<List<BudgetDetailsDto>?> GetAllBudgets(Guid userId)
    {
        try
        {
            var today = DateTime.UtcNow.Date;
            var startDate = today.AddDays(-30);
            var endDate = today;
            
            var response = await dbContext.BudgetMasters
                .AsNoTracking()
                .Where(b => b.UserId == userId && b.IsActive)
                
                .Select(b => new BudgetDetailsDto
                {
                    UserId = b.UserId,
                    BudgetMasterId = b.BudgetMasterId,
                    Amount = b.Amount,
                    UsedAmount = dbContext.TransactionMasters
                        .Where(t=>t.UserId == userId 
                                  && t.TransactionCategoryMasterId == b.CategoryId 
                                  && t.TransactionTypeMasterId == 2 
                                  && t.IsActive == 1 
                                  && t.TransactionDate >= startDate 
                                  && t.TransactionDate <= endDate).Sum(t => t.TransactionAmount) ,
                    
                    
                    UsedPercentage = b.Amount == 0 ? 0 :
                            (
                                dbContext.TransactionMasters
                                    .Where(t =>
                                        t.UserId == userId &&
                                        t.TransactionCategoryMasterId == b.CategoryId &&
                                        t.TransactionTypeMasterId == 2 &&
                                        t.IsActive == 1 &&
                                        t.TransactionDate >= startDate &&
                                        t.TransactionDate <= endDate)
                                    .Sum(t => t.TransactionAmount) 
                            ) / b.Amount * 100,

                    CategoryId = b.CategoryId,

                    CategoryName = dbContext.TransactionCategoryMasters.Where(i=>i.TransactionCategoryMasterId == b.CategoryId).FirstOrDefault().TransactionCategoryName,
                })
                .ToListAsync();

            
            return response;
        }
        catch (Exception e)
        {
            throw new Exception("\"There was an error  data . Please try again.\"", e);
            
        }
    }

  
    public async Task<BudgetDto?> GetBudget(Guid userId, int categoryId)
    {
        try
        {
            var response = await dbContext.BudgetMasters
                .Include(i=>i.TransactionCategoryMaster)
                .Where(i => i.UserId == userId && i.IsActive == true && i.CategoryId == categoryId)
                .Select(i => new BudgetDto
                {
                    UserId   =  i.UserId,
                    BudgetMasterId =   i.BudgetMasterId,
                    CategoryId =   i.CategoryId,
                    CategoryName = i.TransactionCategoryMaster!.TransactionCategoryName ??  null,
                    CreatedAt =   i.CreatedAt,
                    IsActive =  i.IsActive,
                }).FirstOrDefaultAsync( );
            
            return response;
        }
        catch (Exception e)
        {
            throw new Exception("\"There was an error data . Please try again.\"", e);
            
        }
    }
    

    public async Task<BudgetDto> AddNewBudget(BudgetMaster createBudgetDto)
    {
        try
        {
           
            var response  =  await  dbContext.BudgetMasters.AddAsync(createBudgetDto);
            await dbContext.SaveChangesAsync();

            var newEntity = new BudgetDto
            {
                    BudgetMasterId = response.Entity.BudgetMasterId,    
                    CategoryId = response.Entity.CategoryId,    
                    CategoryName = dbContext.TransactionCategoryMasters
                        .Where(i=> i.TransactionCategoryMasterId ==  response.Entity.CategoryId)
                        .Select(i=> i.TransactionCategoryName)
                        .FirstOrDefault(),   
                    CreatedAt =   response.Entity.CreatedAt,
                    IsActive =  response.Entity.IsActive,
                    
            };
            return newEntity;
        }
        catch (Exception e)
        {
            throw new Exception("\"There was an error data . Please try again.\"", e);
        }
    }

    public async Task<bool> DeleteBudget(Guid budgetMasterId)
    {
        try
        {
            
            var existing = await dbContext.BudgetMasters
                .FirstOrDefaultAsync(a => a.BudgetMasterId == budgetMasterId && a.IsActive);

            if (existing == null)
                return false;

            existing.IsActive = false;
            existing.DeletedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            throw new Exception("\"There was an error data . Please try again.\"", e);
        }
    }
}