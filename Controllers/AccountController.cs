using COURSEPROJECT.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using COURSEPROJECT.Dto.Request;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using COURSEPROJECT.Utility;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;





namespace COURSEPROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager ,IEmailSender emailSender,RoleManager<IdentityRole>roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.roleManager = roleManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
        {
            var applicationuser = register.Adapt<ApplicationUser>();
            var result = await userManager.CreateAsync((ApplicationUser)applicationuser, register.Password);
            if (result.Succeeded)
            {
               
                await userManager.AddToRoleAsync(applicationuser, StaticData.Student);
           var token= await userManager.GenerateEmailConfirmationTokenAsync(applicationuser);
                var confirmationLink = Url.Action(nameof(ConfirmEmail),"Account", new { token, userid = applicationuser.Id },
                    protocol: Request.Scheme,
                    host: Request.Host.Value



                ) ;
                     
                
                await emailSender.SendEmailAsync(
    applicationuser.Email,
    "Confirm Your Email",
    $@"
    <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9; color: #333;'>
        <h1 style='color: #4CAF50;'>Hello, {applicationuser.Email}</h1>
        <p style='font-size: 16px;'>Thank you for registering at <strong>Hup Academy</strong>!</p>
        <p style='font-size: 14px;'>To complete your registration, please confirm your email address by clicking the button below:</p>
        <div style='margin: 30px 0; text-align: center;'>
            <a href='{confirmationLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; font-size: 16px;'>Confirm Email</a>
        </div>
        <p style='font-size: 12px; color: #888;'>If you didn’t create this account, you can safely ignore this email.</p>
    </div>"
 );
                

                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("Confirm Email")]
        public async Task <IActionResult> ConfirmEmail(string token,string userid)
        {
            var user=await userManager.FindByIdAsync(userid);

            if (user is not null) {

                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return Ok(new { message = "email confirmed" });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            
            }
             return NotFound();

        }


        [HttpPost("login")]
        public async Task <IActionResult> login( [FromBody] LogInRequest logInRequest)
           
        {
        

            var applictionuser = await userManager.FindByEmailAsync(logInRequest.Email);
            if (applictionuser != null)
            {
               var result= await signInManager.PasswordSignInAsync(applictionuser, logInRequest.Password,logInRequest.RememberMe,false);
                List<Claim> claims = new();
                claims.Add((new("id",applictionuser.Id)));
                claims.Add(new("username", applictionuser.UserName));
               claims.Add(new("email", applictionuser.Email));
                var userRoles= await  userManager.GetRolesAsync(applictionuser);
                if(userRoles.Count > 0) {
                    foreach (var item in userRoles)
                        claims.Add(new("role", item));


                }
              
                if (result.Succeeded)
                {
                    SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes("wUTTqk2HZStu8PTAlAz5npa93FRDhW39"));
                   SigningCredentials signingCredentials=new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

             var JwtToken=       new JwtSecurityToken(
                        claims: claims,
                       expires:DateTime.Now.AddMinutes(30) ,
                       signingCredentials: signingCredentials


                        );
                    string token= new JwtSecurityTokenHandler().WriteToken(JwtToken);


                    return Ok(new { token });
                }
                else
                {
                    if(result.IsLockedOut)
                    return BadRequest(new { message = "your account is locked,please try again later." });
                }
                if(result.IsNotAllowed) {

                    return BadRequest(new { message = "email not confirmed,please confirm your email befor loggin.." });
                }
                
                

                   
            }
            return BadRequest(new { message ="invalid email or password"});
        
        }
        [HttpGet("logout")]
        [Authorize]
        public async Task <IActionResult> Logout()
        {
           await signInManager.SignOutAsync();
            return NoContent();
        }
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            var applictionuser= await userManager.GetUserAsync(User);
            if (applictionuser != null)
            {
              var result  = await userManager.ChangePasswordAsync(applictionuser,changePassword.OldPassword,changePassword.NewPassword);
                if (result.Succeeded)
                {
                    return NoContent();

                }
                return BadRequest(result.Errors);
            }
            return BadRequest(new { message = "invalid email or password" });
        }
    }
}
