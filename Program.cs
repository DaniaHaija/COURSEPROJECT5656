
using COURSEPROJECT.Data;

using COURSEPROJECT.Model;
using COURSEPROJECT.Services;
using COURSEPROJECT.Services.IServices;
using COURSEPROJECT.Utility;
using COURSEPROJECT.Utility.DBInitlizer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace COURSEPROJECT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICategroyService, CategroyService>();

            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
            builder.Services.AddScoped<IRatingService, RatingService>();
            builder.Services.AddScoped<ICourseService, CourseService>();





           
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IDBInitlizer, DBInitlizer>();

            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedEmail = true;
            })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                  ValidateIssuer=false,
                  ValidateAudience=false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wUTTqk2HZStu8PTAlAz5npa93FRDhW39")),
                };
            });




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);


            app.UseAuthorization();


            app.MapControllers();
            var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IDBInitlizer>();
            service.initlizerAsync();

            app.Run();
        }
    }
}
