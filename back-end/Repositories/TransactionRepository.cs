using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget_Tracker_WebAPI.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly Db15765Context db;

        public TransactionRepository(Db15765Context _db)
        {

            this.db = _db;

        }

        public TransactionDto AddTransaction(TransactionMaster transaction)
        {

            try
            {


                var newEntity =  db.TransactionMasters.Add(transaction);
                db.SaveChanges();

                return new TransactionDto
                {
                    TransactionMasterId = newEntity.Entity.TransactionMasterId,
                    TransactionAmount = newEntity.Entity.TransactionAmount,
                    IsActive = newEntity.Entity.IsActive,
                    //TransactionCategory =   db.TransactionCategoryMasters.Where(i=>i.TransactionCategoryMasterId ==    newEntity.Entity.TransactionCategoryMasterId).Select(i=>i.TransactionCategoryName).FirstOrDefault(),
                    TransactionDate = newEntity.Entity.TransactionDate,
                    TransactionCategoryMasterId = newEntity.Entity.TransactionCategoryMasterId,
                    TransactionDescription = newEntity.Entity.TransactionDescription,
                    TransactionNote = newEntity.Entity.TransactionNote,
                    // TransactionType =  db.TransactionTypeMasters.Where(i => i.TransactionTypeMasterId == newEntity.Entity.TransactionTypeMasterId).Select(i => i.TransactionTypename).FirstOrDefault(),
                    TransactionTypeMasterId = newEntity.Entity.TransactionTypeMasterId,
                    UserId = newEntity.Entity.UserId,
                    CreatedAt = newEntity.Entity.CreatedAt

                };

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error add new transaction. Please try again.\"", ex);
            }
        }

        public TransactionCategoryMasterDTO AddTransactionCategory(TransactionCategoryMaster model)
        {

            try
            {


                var newEntity =  db.TransactionCategoryMasters.Add(model);
                db.SaveChanges();

                return new TransactionCategoryMasterDTO
                {
                    TransactionCategoryMasterId = newEntity.Entity.TransactionCategoryMasterId,
                    TransactionCategoryName = newEntity.Entity.TransactionCategoryName,

                };

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error add new transaction. Please try again.\"", ex);
            }
        }

        public  TransactionTypeMasterDTO AddTransactionType(TransactionTypeMaster model)
        {

            try
            {


                var newEntity =  db.TransactionTypeMasters.Add(model);
                db.SaveChanges();

                return new TransactionTypeMasterDTO
                {
                    TransactionTypeMasterId = newEntity.Entity.TransactionTypeMasterId,
                    TransactionTypename = newEntity.Entity.TransactionTypename,


                };

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error add new transaction. Please try again.\"", ex);
            }
        }

        public TransactionPaymentModeDTO AddPaymentMode(TransactionPaymentMode model)
        {

            try
            {


                var newEntity =  db.TransactionPaymentModes.Add(model);
                db.SaveChanges();

                return new TransactionPaymentModeDTO
                {
                    PaymentModeId = newEntity.Entity.PaymentModeId,
                    PaymentMode = newEntity.Entity.PaymentMode,
                    IsActive = 1,
                    IsCustom = true,
                    UserMasterId = newEntity.Entity.UserMasterId


                };

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error add new transaction. Please try again.\"", ex);
            }
        }


        public void DeleteTransactionType(TransactionTypeMaster model)
        {

            try
            {

                model.IsActive = 0;
                db.TransactionTypeMasters.Update(model);
                db.SaveChanges();

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error deleting  transaction type . Please try again.\"", ex);
            }
        }

        public void DeletePaymentMode(TransactionPaymentMode model)
        {

            try
            {

                model.IsActive = 0;
                db.TransactionPaymentModes.Update(model);
                db.SaveChanges();

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error deleting  transaction payement mode . Please try again.\"", ex);
            }
        }

        public void DeleteTransactionCategory(TransactionCategoryMaster model)
        {

            try
            {

                model.IsActive = 0;
                db.TransactionCategoryMasters.Update(model);
                db.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error add new transaction. Please try again.\"", ex);
            }
        }


        public TransactionDto EditTransaction(TransactionMaster transaction)
        {

            try
            {

                var entity = db.TransactionMasters.Update(transaction);
                db.SaveChanges();

                return new TransactionDto
                {
                    TransactionMasterId = entity.Entity.TransactionMasterId,
                    TransactionAmount = entity.Entity.TransactionAmount,
                    IsActive = entity.Entity.IsActive,
                    TransactionCategory = entity.Entity.TransactionCategoryMaster.TransactionCategoryName,
                    TransactionDate = entity.Entity.TransactionDate,
                    TransactionCategoryMasterId = entity.Entity.TransactionCategoryMasterId,
                    TransactionDescription = entity.Entity.TransactionDescription,
                    TransactionNote = entity.Entity.TransactionNote,
                    TransactionType = entity.Entity.TransactionTypeMaster.TransactionTypename,
                    TransactionTypeMasterId = entity.Entity.TransactionTypeMasterId,
                    UserId = entity.Entity.UserId,
                    CreatedAt = entity.Entity.CreatedAt,
                    DeletedAt = entity.Entity.DeletedAt
                };


            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error add new user. Please try again.\"", ex);
            }
        }

        public void Delete(TransactionMaster transaction)
        {

            try
            {
                transaction.IsActive = 0;
                transaction.DeletedAt = DateTime.UtcNow;
                db.TransactionMasters.Update(transaction);
                db.SaveChanges();

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error editing your transaction. Please try again.\"", ex);
            }
        }

        public async Task<List<TransactionDto>> GetAllTransactionByUserID(Guid userId, int page, int pageSize)
        {
            try
            {

                var skip = (page - 1) * pageSize;

                var result = await db.TransactionMasters
                    .Include(i => i.TransactionCategoryMaster)
                    .Include(i => i.TransactionTypeMaster)
                    .Include(i => i.TransactionPaymentmode)
                    .Where(i => i.UserId == userId && i.IsActive == 1)
                    .Select(t => new TransactionDto
                    {

                        TransactionTypeMasterId = t.TransactionTypeMasterId,
                        TransactionDescription = t.TransactionDescription,
                        TransactionAmount = t.TransactionAmount,
                        TransactionDate = t.TransactionDate,
                        TransactionPaymentModeId = (t.TransactionPaymentmode != null) ? t.TransactionPaymentmodeId : null,
                        TransactionPaymentMode = (t.TransactionPaymentmode != null) ? t.TransactionPaymentmode.PaymentMode : null,
                        TransactionCategory = t.TransactionCategoryMaster.TransactionCategoryName,
                        TransactionMasterId = t.TransactionMasterId,
                        TransactionNote = t.TransactionNote,
                        TransactionType = t.TransactionTypeMaster.TransactionTypename,
                        CreatedAt = t.CreatedAt,
                        DeletedAt = t.DeletedAt,
                        UserId = t.UserId,
                        IsActive = t.IsActive,

                        TransactionCategoryMasterId = t.TransactionCategoryMasterId
                    }).OrderByDescending(i => i.CreatedAt).Skip(skip).Take(pageSize).ToListAsync();


                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching  your transactions. Please try again.\"", ex); ;
            }
        }
        public async Task<List<TransactionDto>> GetRecentTransactionsByUserID(Guid userId)
        {
            try
            {
                return await db.TransactionMasters.AsNoTracking()
                    .Include(i => i.TransactionCategoryMaster)
                    .Include(i => i.TransactionTypeMaster)
                    .Include(i => i.TransactionPaymentmode)
                    .Where(i => i.UserId == userId && i.IsActive == 1)
                    .Select(t => new TransactionDto
                    {

                        TransactionTypeMasterId = t.TransactionTypeMasterId,
                        TransactionDescription = t.TransactionDescription,
                        TransactionAmount = t.TransactionAmount,
                        TransactionDate = t.TransactionDate,
                        TransactionCategory = t.TransactionCategoryMaster.TransactionCategoryName,
                        TransactionMasterId = t.TransactionMasterId,
                        TransactionNote = t.TransactionNote,
                        TransactionType = t.TransactionTypeMaster.TransactionTypename,
                        CreatedAt = t.CreatedAt,
                        DeletedAt = t.DeletedAt,
                        UserId = t.UserId,
                        IsActive = t.IsActive,
                        TransactionPaymentModeId = t.TransactionPaymentmodeId ?? null,
                        TransactionPaymentMode = t.TransactionPaymentmode.PaymentMode ?? null,
                        TransactionCategoryMasterId = t.TransactionCategoryMasterId
                    }).OrderByDescending(i => i.CreatedAt).Take(5).ToListAsync();



            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching  your transactions. Please try again.\"", ex); ;
            }
        }
        public TransactionDto GetTransactionByTransactionMasterID(Guid transactionMasterID)
        {
            try
            {
                var transaction = db.TransactionMasters.AsNoTracking()
                    .Include(i => i.TransactionCategoryMaster)
                    .Include(i => i.TransactionTypeMaster)
                    .Where(i => i.TransactionMasterId == transactionMasterID && i.IsActive == 1)
                    .Select(t => new TransactionDto
                    {

                        TransactionTypeMasterId = t.TransactionTypeMasterId,
                        TransactionDescription = t.TransactionDescription,
                        TransactionAmount = t.TransactionAmount,
                        TransactionDate = t.TransactionDate,
                        TransactionCategory = t.TransactionCategoryMaster.TransactionCategoryName,
                        TransactionMasterId = t.TransactionMasterId,
                        TransactionNote = t.TransactionNote,
                        TransactionType = t.TransactionTypeMaster.TransactionTypename,
                        CreatedAt = t.CreatedAt,
                        DeletedAt = t.DeletedAt,
                        UserId = t.UserId,
                        IsActive = t.IsActive,
                        TransactionCategoryMasterId = t.TransactionCategoryMasterId
                    }).FirstOrDefault();

                if (transaction != null)
                {
                    return transaction;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error deleting your transaction. Please try again.\"", ex); ;
            }
        }

        public TransactionTypeMaster GetTransactionTypeByTransactionTypeMasterID(int transactionTypeMasterId)
        {
            try
            {

                // Ensure connection is open

                var model = db.TransactionTypeMasters
                    .AsNoTracking()
                    .Where(i => i.TransactionTypeMasterId == transactionTypeMasterId && i.IsActive == 1)
                    .Select(t => new TransactionTypeMaster
                    {

                        TransactionTypeMasterId = t.TransactionTypeMasterId,
                        TransactionTypename = t.TransactionTypename,
                        UserMasterId = t.UserMasterId,

                        IsActive = t.IsActive,

                    }).FirstOrDefault();

                if (model != null)
                {
                    return model;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching  transaction type. Please try again.\"", ex); ;
            }
        }

        public TransactionCategoryMaster GetTransactionCategoryByTransactionCategoryMasterID(int transactionCategoryMasterId)
        {
            try
            {
                var model = db.TransactionCategoryMasters
                    .AsNoTracking()
                    .Where(i => i.TransactionCategoryMasterId == transactionCategoryMasterId && i.IsActive == 1)
                    .Select(t => new TransactionCategoryMaster
                    {

                        TransactionCategoryMasterId = t.TransactionCategoryMasterId,
                        TransactionCategoryName = t.TransactionCategoryName,
                        UserMasterId = t.UserMasterId,

                        IsActive = t.IsActive,

                    }).FirstOrDefault();

                if (model != null)
                {
                    return model;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching  transaction type. Please try again.\"", ex); ;
            }
        }

        public TransactionPaymentMode GetTransactionPaymentModeByTransactionPayementModeMasterID(int transactionPaymentModeMasterId)
        {
            try
            {
                var model = db.TransactionPaymentModes
                    .AsNoTracking()
                    .Where(i => i.PaymentModeId == transactionPaymentModeMasterId && i.IsActive == 1)
                    .Select(t => new TransactionPaymentMode
                    {

                        PaymentModeId = t.PaymentModeId,
                        PaymentMode = t.PaymentMode,

                        UserMasterId = t.UserMasterId,

                        IsActive = t.IsActive,

                    }).FirstOrDefault();

                if (model != null)
                {
                    return model;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching  transaction type. Please try again.\"", ex); ;
            }
        }


        public async Task<bool> IsTransactionTypePresent(Guid userID, string typeName)
        {
            try
            {
                var transactionType = await db.TransactionTypeMasters.AsNoTracking().FirstOrDefaultAsync(i => i.TransactionTypename.ToLower().Trim() == typeName.ToLower().Trim() && (i.UserMasterId == userID || i.UserMasterId == null) && i.IsActive == 1);

                if (transactionType != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching  transaction type. Please try again.\"", ex); ;
            }
        }

        public async Task<bool> IsTransactionPaymentModePresent(Guid? userID, string paymentMode)
        {
            try
            {
                var transactionType = await db.TransactionPaymentModes.AsNoTracking().FirstOrDefaultAsync(i => i.PaymentMode.ToLower().Trim() == paymentMode.ToLower().Trim() && (i.UserMasterId == userID || i.UserMasterId == null) && i.IsActive == 1);

                if (transactionType != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching  transaction type. Please try again.\"", ex); ;
            }
        }

        public async Task<bool> IsTransactionCategoryPresent(Guid userID, string categoryName)
        {
            try
            {
                var category = await db.TransactionCategoryMasters.AsNoTracking().FirstOrDefaultAsync(i => i.TransactionCategoryName.ToLower().Trim() == categoryName.ToLower().Trim() && (i.UserMasterId == userID || i.UserMasterId == null) && i.IsActive == 1);

                if (category != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching  transaction category. Please try again.\"", ex); ;
            }
        }
        public async Task<List<TransactionCategoryMasterDTO>> GetAllTransactionCategories(Guid userMasterID)
        {
            try
            {
                var transactionCategories = await db.TransactionCategoryMasters
                    .Where(i => i.IsActive == 1 || userMasterID ==  userMasterID)
                    .Select(i => new TransactionCategoryMasterDTO
                    {
                        TransactionCategoryMasterId = i.TransactionCategoryMasterId,
                        TransactionCategoryName = i.TransactionCategoryName,
                        IsCustom = i.UserMasterId != null
                    })
                    .ToListAsync();

                return transactionCategories;
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching categories . Please try again.\"", ex); ;
            }
        }

        public async Task<List<TransactionTypeMasterDTO>> GetAllTransactionTypes(Guid? userMasterID)
        {
            try
            {



                var transactionsTypes = db.TransactionTypeMasters
                    .AsNoTracking()
                    .Where(i => i.IsActive == 1 && (i.UserMasterId == userMasterID || i.UserMasterId == null))
                    .Select(i =>

                    new TransactionTypeMasterDTO
                    {

                        TransactionTypeMasterId = i.TransactionTypeMasterId,
                        TransactionTypename = i.TransactionTypename,
                        IsCustom = (i.UserMasterId != null) ? true : false,
                    }
                    ).ToList();

                return transactionsTypes;
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching types . Please try again.\"", ex); ;
            }
        }

        public async Task<List<TransactionPaymentModeDTO>> GetAllTransactionPaymentMode(Guid? userMasterID)
        {
            try
            {



                var transactionsPayementModes = db.TransactionPaymentModes.AsNoTracking().Where(i => i.IsActive == 1 /*&& (i.UserMasterId == userMasterID || i.UserMasterId == null)*/).Select(i =>

                    new TransactionPaymentModeDTO
                    {

                        PaymentModeId = i.PaymentModeId,
                        PaymentMode = i.PaymentMode,
                        IsActive = 1,
                        IsCustom = (i.UserMasterId != null) ? true : false,
                        UserMasterId = i.UserMasterId ?? null
                    }
                    ).ToList();

                return transactionsPayementModes;
            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching payement modes . Please try again.\"", ex); ;
            }
        }

        public async Task<TransactionTotalDTO> GetTransactionSummary(Guid userid)
        {

            try
            {
                var today = DateTime.UtcNow.Date;
                var yesterday = today.AddDays(-1);

                var weekStart = today.AddDays(-(int)today.DayOfWeek + (today.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));
                var lastWeekStart = weekStart.AddDays(-7);
                var lastWeekEnd = weekStart.AddDays(-1);

                var monthStart = new DateTime(today.Year, today.Month, 1);
                var lastMonth = today.AddMonths(-1);
                var lastMonthStart = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                var lastMonthEnd = monthStart.AddDays(-1);

                var transactions = await db.TransactionMasters.
                    AsNoTracking()
                    .Where(i => i.UserId == userid && i.IsActive == 1 && i.TransactionDate.Date.Month == System.DateTime.Now.Month)
                    .ToListAsync();

                var totalTransactions = await db.TransactionMasters
                    .AsNoTracking()
                    .Where(i => i.UserId == userid && i.IsActive == 1)
                    .ToListAsync();



                var TotalBalance = (float)(totalTransactions.Where(i => i.TransactionTypeMasterId == 1)
                    .Sum(i => i.TransactionAmount) - totalTransactions.Where(i => i.TransactionTypeMasterId == 2)
                    .Sum(i => i.TransactionAmount));

                var TotalExpense = (float)transactions.Where(i => i.TransactionTypeMasterId == 2)
                    .Sum(i => i.TransactionAmount);


                var TotalIncome = (float)transactions.Where(i => i.TransactionTypeMasterId == 1)
                    .Sum(i => i.TransactionAmount);

                // Day
                var todayIncome =(float) transactions.Where(i => i.TransactionTypeMasterId == 1 && i.TransactionDate.Date == today).Sum(i => i.TransactionAmount);
                var yesterdayIncome = (float)transactions.Where(i => i.TransactionTypeMasterId == 1 && i.TransactionDate.Date == yesterday).Sum(i => i.TransactionAmount);

                var todayExpense = (float)transactions.Where(i => i.TransactionTypeMasterId == 2 && i.TransactionDate.Date == today).Sum(i => i.TransactionAmount);
                var yesterdayExpense = (float)transactions.Where(i => i.TransactionTypeMasterId == 2 && i.TransactionDate.Date == yesterday).Sum(i => i.TransactionAmount);

                // Week
                var thisWeekIncome = (float)transactions.Where(i => i.TransactionTypeMasterId == 1 && i.TransactionDate.Date >= weekStart).Sum(i => i.TransactionAmount);
                var lastWeekIncome = (float)transactions.Where(i => i.TransactionTypeMasterId == 1 && i.TransactionDate.Date >= lastWeekStart && i.TransactionDate.Date <= lastWeekEnd).Sum(i => i.TransactionAmount);

                var thisWeekExpense = (float)transactions.Where(i => i.TransactionTypeMasterId == 2 && i.TransactionDate.Date >= weekStart).Sum(i => i.TransactionAmount);
                var lastWeekExpense = (float)transactions.Where(i => i.TransactionTypeMasterId == 2 && i.TransactionDate.Date >= lastWeekStart && i.TransactionDate.Date <= lastWeekEnd).Sum(i => i.TransactionAmount);

                // Month
                var thisMonthIncome = (float)transactions.Where(i => i.TransactionTypeMasterId == 1 && i.TransactionDate.Date >= monthStart).Sum(i => i.TransactionAmount);
                var lastMonthIncome = (float)transactions.Where(i => i.TransactionTypeMasterId == 1 && i.TransactionDate.Date >= lastMonthStart && i.TransactionDate.Date <= lastMonthEnd).Sum(i => i.TransactionAmount);

                var thisMonthExpense = (float)transactions.Where(i => i.TransactionTypeMasterId == 2 && i.TransactionDate.Date >= monthStart).Sum(i => i.TransactionAmount);
                var lastMonthExpense = (float)transactions.Where(i => i.TransactionTypeMasterId == 2 && i.TransactionDate.Date >= lastMonthStart && i.TransactionDate.Date <= lastMonthEnd).Sum(i => i.TransactionAmount);

                float CalculatePercent(float current, float previous) =>
                 previous != 0 ? ((current - previous) / Math.Abs(previous)) * 100 : 0;

                var dayIncomeChange = CalculatePercent(todayIncome, yesterdayIncome);
                var weekIncomeChange = CalculatePercent(thisWeekIncome, lastWeekIncome);
                var monthIncomeChange = CalculatePercent(thisMonthIncome, lastMonthIncome);

                var dayExpenseChange = CalculatePercent(todayExpense, yesterdayExpense);
                var weekExpenseChange = CalculatePercent(thisWeekExpense, lastWeekExpense);
                var monthExpenseChange = CalculatePercent(thisMonthExpense, lastMonthExpense);

                // Create result object
                var result = new TransactionTotalDTO
                {
                    UserId = userid,
                    CurrentBalance = TotalBalance.ToString("N2"),
                    //PreviousBalance = previousBalance.ToString("N2"),
                    //BalancePercentageChange = Math.Round(balancePercentageChange, 1).ToString("N1"),
                    TodayIncome = todayIncome.ToString("N2"),
                    WeekIncome = thisWeekIncome.ToString("N2"),
                    MonthIncome = thisMonthIncome.ToString("N2"),
                    DayIncomeChange = Math.Round(dayIncomeChange, 1).ToString("N1"),
                    WeekIncomeChange = Math.Round(weekIncomeChange, 1).ToString("N1"),
                    MonthIncomeChange = Math.Round(monthIncomeChange, 1).ToString("N1"),

                    TodayExpense = todayExpense.ToString("N2"),
                    WeekExpense = thisWeekExpense.ToString("N2"),
                    MonthExpense = thisMonthExpense.ToString("N2"),
                    DayExpenseChange = Math.Round(dayExpenseChange, 1).ToString("N1"),
                    WeekExpenseChange = Math.Round(weekExpenseChange, 1).ToString("N1"),
                    MonthExpenseChange = Math.Round(monthExpenseChange, 1).ToString("N1"),

                    CurrentExpense = TotalExpense.ToString("N2"),
                    //PreviousExpense = previousExpense.ToString("N2"),
                    //ExpensePercentageChange = Math.Round(expensePercentageChange, 1).ToString("N1"),

                    CurrentIncome = TotalIncome.ToString("N2"),
                    //PreviousIncome = previousIncome.ToString("N2"),
                    //IncomePercentageChange = Math.Round(incomePercentageChange, 1).ToString("N1")
                };

                return result;

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching types . Please try again.\"", ex); ;
            }
        }
        public async Task<int> GetCountOfTransactions(Guid userId, int numberOfMonths)
        {
            try
            {
                var currentDate = System.DateTime.Now;
                var fromDate = currentDate.AddMonths(-numberOfMonths);

                int transactionCount = await db.TransactionMasters.Where(i => i.UserId == userId && i.TransactionDate <= currentDate && i.TransactionDate >= fromDate && i.IsActive == 1).CountAsync();


                return transactionCount;

            }
            catch (Exception ex)
            {

                throw new Exception("\"There was an error fetching count of the transaction . Please try again.\"", ex);
            }
        }

        public async Task<(List<TransactionDto> Transactions, int TotalItems)> GetPaginatedTransactionsByUserID(Guid userId, TransactionQueryParameters queryParams)
        {
            // Start with a base query
            var query = db.TransactionMasters
                .Include(i => i.TransactionTypeMaster)
                .Include(i => i.TransactionPaymentmode)

                .Include(i => i.TransactionCategoryMaster)
                .Where(t => t.UserId == userId && t.IsActive ==1);

            // Apply Filters
            if (queryParams.StartDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate >= queryParams.StartDate.Value);
            }
            if (!string.IsNullOrEmpty(queryParams.FilterCategory))
            {
                query = query.Where(t => t.TransactionCategoryMaster.TransactionCategoryName == queryParams.FilterCategory);
            }
            if (!string.IsNullOrEmpty(queryParams.SearchText))
            {
                query = query.Where(t => t.TransactionDescription.Contains(queryParams.SearchText));
            }
            // ... add all other filters ...

            // Get the total count AFTER filtering but BEFORE pagination
            var totalItems = await query.CountAsync();

            // Apply Sorting
            // This part can be complex. A simple example:
            if (queryParams.SortDirection?.ToLower() == "desc")
            {
                query = query.OrderByDescending(t => t.TransactionDate); // Simplified - you'd need a switch for other columns
            }
            else
            {
                query = query.OrderBy(t => t.TransactionDate);
            }

            // Apply Pagination
            var transactions = await query
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Select(t => new TransactionDto
                {

                    TransactionMasterId = t.TransactionMasterId,
                    TransactionAmount = t.TransactionAmount,
                    TransactionCategory = t.TransactionCategoryMaster.TransactionCategoryName,
                    TransactionCategoryMasterId = t.TransactionCategoryMasterId,
                    TransactionDate = t.TransactionDate,
                    TransactionDescription = t.TransactionDescription,
                    TransactionNote = t.TransactionNote,
                    TransactionPaymentMode = t.TransactionPaymentmode.PaymentMode ?? null,
                    TransactionPaymentModeId = t.TransactionPaymentmodeId ?? null,
                    TransactionType = t.TransactionTypeMaster.TransactionTypename,
                    TransactionTypeMasterId = t.TransactionTypeMasterId,
                    CreatedAt = t.CreatedAt,
                    DeletedAt = t.DeletedAt,
                    IsActive = t.IsActive,
                    UserId = t.UserId

                })
                .ToListAsync();

            return (transactions, totalItems);
        }
    }
}
