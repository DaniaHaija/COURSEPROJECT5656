using COURSEPROJECT.Data;
using COURSEPROJECT.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace COURSEPROJECT.Utility.DBInitlizer
{
    public class DBInitlizer : IDBInitlizer
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public DBInitlizer(ApplicationDbContext context,RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task initlizerAsync()

        {
            try
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
            catch (Exception e)
            {
              Console.WriteLine(e.Message);
            }
            if (roleManager.Roles is not null)
            {
                await roleManager.CreateAsync(new(StaticData.Admin));
                await roleManager.CreateAsync(new(StaticData.Moderator));
                await roleManager.CreateAsync(new(StaticData.Student));
                await userManager.CreateAsync(new()
                {

                    UserName = "admin123",
                    Email = "balimi90377@gmail.com",
                }

              , "Admin@1boss");
                var user = await userManager.FindByEmailAsync("balimi90377@gmail.com");

                await userManager.AddToRoleAsync(user, StaticData.Admin);

            }
           


        }
    }
}
