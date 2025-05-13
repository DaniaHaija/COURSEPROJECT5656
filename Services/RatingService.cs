using COURSEPROJECT.Data;
using COURSEPROJECT.Model;
using COURSEPROJECT.Services.IServices;

namespace COURSEPROJECT.Services
{
    public class RatingService(ApplicationDbContext context) : Service<Rating>(context), IRatingService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<bool> EditAsync(int id,Rating rating, CancellationToken cancellationToken = default)
        {
            Rating? ratingInDb = _context.Ratings.Find(id);
            if (ratingInDb == null) return false;

            ratingInDb.Score = rating.Score;
            ratingInDb.Comment = rating.Comment;

            await _context.SaveChangesAsync();
            return true;






        }
    }
}

