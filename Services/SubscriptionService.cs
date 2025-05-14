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
                context.SaveChanges();
             
               



            }
            return existingSubscriptionitems;

        }
   
       
            public async Task<IEnumerable<Subscription>> GetUserSubscriptionsAsync(int CourseId)
            {
                var subscriptions = await context.Subscriptions
                    .Where(s => s.CourseId == CourseId ) 
                    .Include(s => s.User) 
                    .ToListAsync();

                    return subscriptions;
               
            }

        public async Task<IEnumerable<Subscription>> GetUserSubscriptionsUserAsync(string UserId)
        {
            var subscriptions=await  context.Subscriptions.Where(s=>s.UserId == UserId).Include(s => s.Course).Include(s=>s.User).ToListAsync();
            return subscriptions;
        }
    }
    }
