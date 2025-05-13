using COURSEPROJECT.Dto;
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
    [Authorize(Roles = $"{StaticData.Admin}")]
    public class UesersController : ControllerBase
    {
        private readonly IUserService userService;

        public UesersController(IUserService userService) {
            this.userService = userService;
        }
      
        [HttpGet("UserWithRole")]
        public async Task<IActionResult> UserWithRole()
        {
           var result= await userService.UserWithRole();
            return Ok(result);
        }




        [HttpPut("{userId}")]
        public async Task<IActionResult> ChangeRole([FromRoute]  string userId, [FromQuery] string roleName)
        {
        var result= await userService.ChangeRoleAsync(userId, roleName);
            return Ok(result);
        }

        [HttpPatch("LoukUnLouk/{id}")]
        public async Task<IActionResult> LockUnlock(string id)
        {
            var result= await userService.LockUnlockAsync(id);
            if(result== true)
            {
                return Ok();
            }
            return BadRequest();
        }
        
    }
}
