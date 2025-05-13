using COURSEPROJECT.Data;
using COURSEPROJECT.Dto;
using COURSEPROJECT.Model;
using COURSEPROJECT.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;


namespace COURSEPROJECT.Services
{
    public class UserService : Service<ApplicationUser>,IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser>  _userManager;
        public UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager):base(context)
        {
           this._context = context;
            this._userManager = userManager;
        }

        public async Task<List<UserDto>> UserWithRole()
        {
            var users = _userManager.Users.ToList();
            var userwithroles=new List<UserDto>();
            foreach (var user in users)
            {
                var roles=await _userManager.GetRolesAsync(user);
                var dto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList()
                };
                userwithroles.Add(dto);
            }
            return userwithroles;
        }
        public async Task<bool> ChangeRoleAsync(string userId, string roleName)
        {
            var user= await _userManager.FindByIdAsync(userId);
            if (user is not null) { 
              
                var OldRoles= await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, OldRoles);

                var result = await _userManager.AddToRoleAsync(user, roleName);
                if(result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            
            }
            return false;
        }

        public async Task<bool?> LockUnlockAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return null;
            var isLockedNow=user.LockoutEnabled && user.LockoutEnd > DateTime.Now;
            if (isLockedNow)
            {
                user.LockoutEnabled = false;
                user.LockoutEnd = null;
            }
            else
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.Now.AddMinutes(1);
            }
            await _userManager.UpdateAsync(user);
            return !isLockedNow;    
        }
    }
 
}
