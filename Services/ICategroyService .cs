using COURSEPROJECT.Model;
using COURSEPROJECT.Services.IServices;
using System.Linq.Expressions;

namespace COURSEPROJECT.Services
{
    public interface ICategroyService : IService<Category>
    {
        

       Task <bool> EditAsync( int id,Category category, CancellationToken cancellationToken = default);
      
    }
}
