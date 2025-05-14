using COURSEPROJECT.Data;
using COURSEPROJECT.Dto.Request;
using COURSEPROJECT.Model;
using COURSEPROJECT.Services.IServices;
using Mapster;



namespace COURSEPROJECT.Services
{
    public class RatingService(ApplicationDbContext context) : Service<Rating>(context), IRatingService
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<bool> EditAsync(int id,string UserId ,Rating rating, CancellationToken cancellationToken = default)
        {
            Rating? ratingInDb = _context.Ratings.Find(id);
            if (ratingInDb == null) return false;

            ratingInDb.Score = rating.Score;
            ratingInDb.Comment = rating.Comment;
            ratingInDb.UserId=UserId;

            await _context.SaveChangesAsync();
            return true;






        }



    }


}

    


