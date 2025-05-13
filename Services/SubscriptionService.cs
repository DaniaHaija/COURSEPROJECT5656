using COURSEPROJECT.Data;
using COURSEPROJECT.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace COURSEPROJECT.Services.IServices
{
    public class SubscriptionService : Service<Subscription>, ISubscriptionService
    {
        private readonly ApplicationDbContext context;

        public SubscriptionService(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Subscription> AddSubcription(string UserId,int CourseId,CancellationToken cancellationToken)
        {
            var existingSubscriptionitems= await context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == UserId && s.CourseId == CourseId);
            if (existingSubscriptionitems != null)
            {
                return existingSubscriptionitems;
            }
            else {

                existingSubscriptionitems = new Subscription()
                {
                    CourseId = CourseId,

                    UserId = UserId
                };

                await context.Subscriptions.AddAsync(existingSubscriptionitems);
             
               



            }
            return existingSubscriptionitems;

        }
    public async Task<IEnumerable<Subscription>> GetUserSubscriptionsAsync(string UserId)
        {
            return await GetAsync();

        }

    }
}
