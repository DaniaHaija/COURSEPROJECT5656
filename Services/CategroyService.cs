using COURSEPROJECT.Data;
using COURSEPROJECT.Model;
using COURSEPROJECT.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;

namespace COURSEPROJECT.Services
{
    public class CategroyService(ApplicationDbContext context) : Service<Category>(context),ICategroyService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<bool> EditAsync(int id, Category category, CancellationToken cancellationToken = default)
        {
           Category? categoryInDb = _context.Categories.Find(id);
            if (categoryInDb == null) return false;
           
            categoryInDb.Name = category.Name;
            await _context.SaveChangesAsync();
            return true;






        }
    }
}
