using COURSEPROJECT.Data;

using COURSEPROJECT.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace COURSEPROJECT.Services.IServices
    
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbset;
        public Service(ApplicationDbContext context) {
            this._context = context;
            _dbset=_context.Set<T>();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<T?> GetOneAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>>?[] includes = null, bool IsTracked = true)
        {
            var all =await GetAsync(expression, includes, IsTracked);
            return all.FirstOrDefault();
        }

        public async  Task <IEnumerable<T>> GetAsync(Expression<Func<T, bool>> ?expression=null, Expression<Func<T, object>>?[] includes = null, bool IsTracked = true)
        {
            IQueryable<T> entites = _dbset;
            if(expression is not null)
            {
                entites = entites.Where(expression);
            }
            if(includes != null)
            {
                foreach (var Item in includes)
                { 
                 entites=entites.Include(Item);  
                
                }
            }
            if(!IsTracked)
            {
                entites=entites.AsNoTracking();

            }
            return await entites.ToListAsync();

        }

       
        

            public async Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = _dbset.Find(id);
            if (entity == null)
            {
                return false;
            }
            _dbset.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
