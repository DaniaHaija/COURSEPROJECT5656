using COURSEPROJECT.Model;
using COURSEPROJECT.Services.IServices;

namespace COURSEPROJECT.Services
{
    public interface ISubscriptionService: IService<Subscription>
    {
         Task<Subscription> AddSubcription(string UserId, int CourseId, CancellationToken cancellationToken);
        Task<IEnumerable<Subscription>> GetUserSubscriptionsAsync( int CourseId);
        Task<IEnumerable<Subscription>> GetUserSubscriptionsUserAsync(String UserId);

    }
}
