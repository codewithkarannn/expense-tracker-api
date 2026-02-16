using Budget_Tracker_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Budget_Tracker_WebAPI.Repositories
{
    public class Repository<T>  : IRepository<T> where T : class
    {
        private readonly DbExpenseTrackerContext _context;
        private readonly DbSet<T> _db;

        public Repository(DbExpenseTrackerContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _db.ToListAsync();

        public async Task<T> GetByIdAsync(int id) => await _db.FindAsync(id);

        public async Task AddAsync(T entity)
        {
            await _db.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.FindAsync(id);
            if (entity != null)
            {
                _db.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) =>
            await _db.AnyAsync(predicate);


    }
}
