using COURSEPROJECT.Dto;
using COURSEPROJECT.Model;
using COURSEPROJECT.Services.IServices;
using System.Linq.Expressions;

namespace COURSEPROJECT.Services
{
    public interface IUserService : IService<ApplicationUser>
    {
        Task<List<UserDto>> UserWithRole();
        Task<bool> ChangeRoleAsync(string userId, string roleName);
        Task<bool?> LockUnlockAsync(string id);




    }
}
