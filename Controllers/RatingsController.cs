using COURSEPROJECT.Dto.Request;
using COURSEPROJECT.Dto.Response;
using COURSEPROJECT.Model;
using COURSEPROJECT.Services;
using COURSEPROJECT.Utility;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace COURSEPROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RatingsController(IRatingService ratingService,UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly IRatingService ratingService = ratingService;
        private readonly UserManager<ApplicationUser> userManager = userManager;

        [HttpPost("{CourseId}")]
        
        public async Task <IActionResult> AddRating([FromRoute] int CourseId, [FromBody] RatingRequest ratingRequest )
        {
            var appUser = userManager.GetUserId(User);

            var Rating = new Rating()
            {
                CourseId = CourseId,
       
                Comment = ratingRequest.Comment,
                Score = ratingRequest.Score,
                UserId=ratingRequest.UserId,

            };
         var rating=   Rating.Adapt<Rating>();
            await ratingService.AddAsync(rating);
            return Ok(rating);

        }
        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRating()
        {
       var ratings=   await  ratingService.GetAsync(null,[r => r.User, r => r.Course],true);
            return Ok(ratings.Adapt<IEnumerable<RatingResponse>>());
        }
        [HttpGet("{id}")]

        [AllowAnonymous]
        public async Task <IActionResult> GetRatingById(int id)
        {
            var rating = await ratingService.GetOneAsync(r => r.ID == id, [r=>r.User,r=>r.Course]);
            if (rating == null) return NotFound();

            return Ok(rating.Adapt<RatingResponse>());
          

        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
          await  ratingService.RemoveAsync(id);
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] RatingRequest ratingRequest)
        {
            var ratingInD = await ratingService.EditAsync(id, ratingRequest.Adapt<Rating>());

            if (!ratingInD) return NotFound();

            return NoContent();


        }
    }
}
