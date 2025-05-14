using COURSEPROJECT.Data;
using COURSEPROJECT.Model;
using COURSEPROJECT.Services;
using COURSEPROJECT.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading;
using Mapster;
using COURSEPROJECT.Services.IServices;
using COURSEPROJECT.Dto.Response;
using Microsoft.AspNetCore.Http.HttpResults;



namespace COURSEPROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionsController(ISubscriptionService subscriptionService ,UserManager<ApplicationUser> userManager, IEmailSender emailSender) : ControllerBase
    {
        private readonly ISubscriptionService subscriptionService = subscriptionService;
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly IEmailSender emailSender = emailSender;
        

        [HttpPost("{CourseId}")]
        public async Task <IActionResult> AddSubcription([FromRoute] int CourseId, CancellationToken cancellationToken)
        {
            var appUser = User.FindFirst("id").Value;
            var subscription= await subscriptionService.AddSubcription(appUser, CourseId, cancellationToken);
            var subscriptionResponse = subscription.Adapt<SubscriptionResponse>();


            return Ok(subscriptionResponse);
        }
        [HttpGet("{CourseId}")]
        [Authorize(Roles = $"{StaticData.Admin}")]
        public async Task<IActionResult> GetUserSubscriptionsAsync([FromRoute] int CourseId)
        {
            var appUser = User.FindFirst("id").Value;

            var result= await subscriptionService.GetUserSubscriptionsAsync(CourseId);

            var resultResponse = result.Adapt<IEnumerable<SubscriptionResponse>>();
            return Ok(resultResponse);  
        }
        [HttpGet("")]
        [Authorize(Roles = $"{StaticData.Student}")]

        public async Task<IActionResult> GetUserSubscriptionsAsync()
        {
            var userapp = User.FindFirst("id").Value;
            var result=await subscriptionService.GetUserSubscriptionsUserAsync(userapp);
            var resultResponse= result.Adapt<IEnumerable<SubscriptionResponse>>();
            return Ok(resultResponse);
        }


    }
}
