using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;
using Budget_Tracker_WebAPI.Services.BudgetService;
using Microsoft.AspNetCore.Mvc;

namespace Budget_Tracker_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController(IBudgetService budgetService) : ControllerBase
    {
        
        [HttpPost("addNewBudget")]
        public async Task<IActionResult> CreateBudget(CreateBudgetDto createBudgetDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var responses = new ResponseModel<object>("Validation failed", StatusCodes.Status400BadRequest);
                    return BadRequest(responses);
                }

                var addedBudget = await budgetService.AddNewBudget(createBudgetDto);

                var response = new ResponseModel<BudgetDto>(addedBudget, "New budget added successfully", 201);
                return Ok( response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("getAllBudget/{userId}")]
        public async Task<IActionResult> GetAllBudgets(Guid userId)
        {
            try
            {
                var budgets = await budgetService.GetAllBudgets(userId);

                var response = new ResponseModel<List<BudgetDetailsDto?>>(budgets, "Budget fetched successfully", 201);
                return Ok( response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpPut("deleteBudget/{budgetMasterId}")]
        public async Task<IActionResult> DeleteBudget(Guid budgetMasterId)
        {
            try
            {
                var budgets = await budgetService.DeleteBudget(budgetMasterId);

                var response = new ResponseModel<bool>(budgets, "Budget deleted successfully", 201);
                return Ok( response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }
    } 
}
