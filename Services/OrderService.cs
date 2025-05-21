using COURSEPROJECT.Data;
using COURSEPROJECT.Model;
using COURSEPROJECT.Services.IServices;
using System.Linq.Expressions;

namespace COURSEPROJECT.Services
{
    public class OrderService(ApplicationDbContext context) : Service<Order>(context), IOrderService
    {
        private readonly ApplicationDbContext context = context;
    }
}
