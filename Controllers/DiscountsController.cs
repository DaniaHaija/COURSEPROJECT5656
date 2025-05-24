using COURSEPROJECT.Dto.Request;
using COURSEPROJECT.Model;
using COURSEPROJECT.Services;
using COURSEPROJECT.Utility;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COURSEPROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{StaticData.Admin}")]
    public class DiscountsController(IDiscount DiscountService) : ControllerBase
    {
        private readonly IDiscount discountService = DiscountService;

        [HttpPost("create")]
        public async Task<IActionResult> CreateDiscount([FromBody] DiscountRequest discountRequest)
        {

         var discount=discountRequest.Adapt<Discount>();

        var resualt= await discountService.AddAsync(discount);
        return Ok(resualt);
        
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount([FromRoute]int id)
        {
            var discount =await discountService.RemoveAsync(id);

            if (discount == null)
                return NotFound("Discount not found");

            else { return Ok("Discount deleted successfully"); }

           
        }
        [HttpGet("")]
        public async Task<IActionResult> GetDiscount()
        {
            var discounts = await discountService.GetAsync();
            return Ok(discounts);
        }

    }



}

