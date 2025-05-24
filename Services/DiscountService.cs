using COURSEPROJECT.Data;
using COURSEPROJECT.Model;
using COURSEPROJECT.Services.IServices;

namespace COURSEPROJECT.Services
{
    public class DiscountService(ApplicationDbContext context) : Service<Discount>(context), IDiscount

    {
        private readonly ApplicationDbContext context = context;
    }
}
