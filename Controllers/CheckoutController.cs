//using COURSEPROJECT.Model;
//using COURSEPROJECT.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Stripe.Checkout;

////namespace COURSEPROJECT.Controllers
////{
////    [Route("api/[controller]")]
//    //[ApiController]
//    //[Authorize]
////    public class CheckoutController(ISubscriptionService subscriptionService) : ControllerBase
////    {
////        private readonly ISubscriptionService subscriptionService = subscriptionService;
////        [HttpGet("pay")]
////        public async Task<IActionResult> pay()
////        {
////            var userapp = User.FindFirst("id").Value;
////            var subscriptions = await subscriptionService.GetUserSubscriptionsUserAsync(userapp);

////            if (subscriptions is not null)

////            {
////                Order order = new()
////                {
////                    orderStatus= OrderStatus.Pending,
////                    OrderDate = DateTime.Now,
////                    OriginalPrice =  foreach(var subcription in subscriptions)
////                {
////                    subcription.Course.Price
////                }

////                }
////                var options = new SessionCreateOptions
////                {
////                    PaymentMethodTypes = new List<string> { "card" },
////                    LineItems = new List<SessionLineItemOptions>(),
////                    Mode = "payment",
////                    SuccessUrl = $"{Request.Scheme}://{Request.Host}/api/Checkout/success",
////                    CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
////                };

////                foreach (var item in subscriptions)
////                {
////                    options.LineItems.Add(
////                        new SessionLineItemOptions
////                        {
////                            PriceData = new SessionLineItemPriceDataOptions
////                            {
////                                Currency = "USD",
////                                ProductData = new SessionLineItemPriceDataProductDataOptions
////                                {
////                                    Name = item.Course.Title,
////                                    Description = item.Course.Description,
////                                },
////                                UnitAmount = (long)(item.Course.Price * 100), 

////                            },
////                            Quantity=1,
////                        });
////                }

////                var service = new SessionService();
////                var session = service.Create(options);
////                return Ok(new { session.Url });
////            }
////            else
////            {
////                return NotFound();
////            }
////        }

////        [HttpGet("success")]
////        public IActionResult Success()
////        {
////            return Ok(new
////            {
////                message = "done"
////            });
////        }
////    }
////}


//    }}