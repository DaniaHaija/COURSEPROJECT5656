using COURSEPROJECT.Data;
using COURSEPROJECT.Model;
using COURSEPROJECT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using COURSEPROJECT.Model;
using COURSEPROJECT.Utility;
using Microsoft.AspNetCore.Identity;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading;


namespace COURSEPROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutController(ISubscriptionService subscriptionService, ApplicationDbContext context, IOrderService orderService, IEmailSender emailSender
, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly ISubscriptionService subscriptionService = subscriptionService;
        private readonly ApplicationDbContext context = context;
        private readonly IOrderService orderService = orderService;
        private readonly IEmailSender emailSender = emailSender;
        private readonly UserManager<ApplicationUser> userManager = userManager;

        [HttpGet("pay/{CourseId}")]
        public async Task<IActionResult> Pay([FromRoute] int CourseId, [FromQuery] string? discountCode = null)
        {
            var userapp = User.FindFirst("id").Value;
            var course = await context.Courses.FindAsync(CourseId);

            if (course == null)
                return NotFound("Course not found");

            var subscriptions = await subscriptionService.GetOneSubscriptionAsync(CourseId, userapp);
            if (subscriptions is not null)
                return BadRequest("You are already subscribed to this course");

            decimal finalPrice = course.Price;
            COURSEPROJECT.Model.Discount? discount = null;

            if (!string.IsNullOrEmpty(discountCode))
            {
                discount = await context.Discounts
                    .FirstOrDefaultAsync(d => d.Code == discountCode && d.ExpiryDate > DateTime.Now);

                if (discount != null)
                {
                    finalPrice = course.Price - discount.Value;

                    if (finalPrice < 0)
                        finalPrice = 0;
                }
            }


            Order order = new()
            {
                orderStatus = OrderStatus.Pending,
                OrderDate = DateTime.Now,
                OriginalPrice = course.Price,
                FinalPrice = finalPrice,
                UserId = userapp,
                CourseId = course.ID,
                DiscountId = discount?.ID
            };
            await orderService.AddAsync(order);


            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)(finalPrice * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = course.Title,
                        Description = course.Description,
                    }
                },
                Quantity = 1,
            }
        },
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/api/Checkout/success/{order.Id}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };

            var service = new SessionService();
            var session = service.Create(options);

            order.SessionId = session.Id;
         await  orderService.CommitAsync();

            return Ok(new { session.Url });
        }


        [HttpGet("success/{OrderId}")]
        [AllowAnonymous]
        public async Task <IActionResult> Success([FromRoute] int OrderId,CancellationToken cancellationToken)
        {
            var order= await orderService.GetOneAsync(o=>o.Id==OrderId);
            var applicationuser=await  userManager.FindByIdAsync(order.UserId);
            var subscription = await subscriptionService.GetOneSubscriptionAsync(order.CourseId, order.UserId);
            if (subscription == null)
            {
                await subscriptionService.AddSubcription(order.UserId, order.CourseId, cancellationToken);
            }
                order.orderStatus = OrderStatus.Approved;
            var service = new SessionService();
            var session =service.Get(order.SessionId);
            order.TransactionId = session.PaymentIntentId;
            await orderService.CommitAsync();
            await emailSender.SendEmailAsync(
              applicationuser.Email,
              "Payment Confirmation - Hup Academy",
              $@"
<div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9; color: #333;'>
    <h1 style='color: #4CAF50;'>Hello, {applicationuser.UserName}</h1>
    <p style='font-size: 16px;'>We’re excited to inform you that your payment has been successfully completed.</p>
    <p style='font-size: 14px;'>Here are your order details:</p>
    <ul style='font-size: 14px;'>
        <li><strong>Order ID:</strong> {order.Id}</li>
     
    </ul>
    <p style='font-size: 14px;'>Thank you for purchasing from <strong>Hup Academy</strong>! You can now access your course from your dashboard.</p>
    <p style='font-size: 12px; color: #888;'>If you have any questions or didn’t make this purchase, please contact our support team.</p>
</div>"
          );
         


            return Ok(new
            {
                message = "done"
            });
        }
    }
}