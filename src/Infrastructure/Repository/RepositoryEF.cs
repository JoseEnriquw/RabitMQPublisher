using Core.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public class RepositoryEF : IRepositoryEF
    {
        private readonly ILogger<RepositoryEF> _logger;
        private readonly DbContext _dbContext;
        public RepositoryEF(ILogger<RepositoryEF> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public void Insert<T>(T entity) where T : class
        {
            try
            {
                _dbContext.Add(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting entity {entity}", entity);
                throw;
            }
        }

        public void Update<T>(T entity) where T : class
        {
            try
            {
                _dbContext.Attach(entity).State = EntityState.Modified;
                _dbContext.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity {entity}", entity);
                throw;
            }
        }

        public void Delete<T>(T entity) where T : class
        {
            try
            {
                _dbContext.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity {entity}", entity);
                throw;
            }
        }

        public async Task<List<T>> GetAllAsync<T>() where T : class
        {
            try
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all ");
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes");
                throw;
            }
        }

        public async Task<T> FindAsync<T>(Expression<Func<T, bool>> func) where T : class
        {
            try
            {
                return await _dbContext.Set<T>().FirstAsync(func);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding entity ");
                throw;
            }
        }
    }
}
