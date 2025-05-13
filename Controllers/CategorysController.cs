using COURSEPROJECT.Data;
using COURSEPROJECT.Dto.Request;
using COURSEPROJECT.Dto.Response;
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
    
    public class CategorysController(ICategroyService categroyService) : ControllerBase
    {
        private readonly ICategroyService CategroyService = categroyService;

        

          [HttpGet]
        [AllowAnonymous]
        public async Task <IActionResult> GetAll()
        {
            var categories = await  categroyService.GetAsync();
            return Ok(categories.Adapt<IEnumerable<CategroyResponse>>());
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task <IActionResult> GetById([FromRoute] int id)
        {
            var category = await categroyService.GetOneAsync(e => e.id == id, [e => e.Courses]);
          
            
            if (category == null) return NotFound();

            return Ok(category.Adapt<CategroyResponse>());
        }
        [HttpPost("")]
        
        public async Task <IActionResult> Create([FromBody] CategroyRequest category,CancellationToken cancellationToken)
        {
          var categroyInAd=await categroyService.AddAsync(category.Adapt<Category>(), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { categroyInAd.id }, categroyInAd);
        }
        [HttpDelete("{id}")]
       
        public async Task <IActionResult> Delete([FromRoute] int id)
        {
            var categoryInDb =  await categroyService.RemoveAsync(id);
            if (!categoryInDb)
                return NotFound();
            return NoContent(); 
        }
        [HttpPut("{id}")]
        
        public async Task <IActionResult> Update( [FromRoute] int id, [FromBody] CategroyRequest category)
        {
            var categoryInD= await categroyService.EditAsync(id, category.Adapt<Category>());    
            if (!categoryInD) return NotFound();
             
            return NoContent();

        }


    }
}