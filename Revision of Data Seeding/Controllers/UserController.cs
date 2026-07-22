using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Revision_of_Data_Seeding.DTOs;
using Revision_of_Data_Seeding.Identity;
using Revision_of_Data_Seeding.Interfaces;
using Revision_of_Data_Seeding.Models;

namespace Revision_of_Data_Seeding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly PesonsDbContext pesonsDbContext;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ITokenGenerator tokenGenerator;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            PesonsDbContext pesonsDbContext,
            RoleManager<ApplicationRole> roleManager,
            ITokenGenerator tokenGenerator
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.pesonsDbContext = pesonsDbContext;
            this.roleManager = roleManager;
            this.tokenGenerator = tokenGenerator;
        }



        [HttpPost("Regitser")]
        public async Task<IActionResult> Register(RegitserDTO regitserDTO)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = regitserDTO.Email,
                Email = regitserDTO.Email,
                PhoneNumber = regitserDTO.PhoneNumber,
            };

            IdentityResult identityResult = await userManager.CreateAsync(user, regitserDTO.Password);

            if (identityResult.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                if (await roleManager.FindByNameAsync("Admin") is null)
                {
                    ApplicationRole role = new ApplicationRole()
                    {
                        Name = "Admin"
                    };
                    await roleManager.CreateAsync(role);

                    await userManager.AddToRoleAsync(user, "Admin");
                }


                return Created();
            }

            return BadRequest(identityResult.Errors);
        }


        //[HttpPost("signin")]
        //public async Task<IActionResult> Signin(SigninDTO signinDTO)
        //{

        //    //var user = await userManager.FindByEmailAsync(signinDTO.username);
        //    //if (user == null) 
        //    //    return Unauthorized();


        //    var result = await signInManager.PasswordSignInAsync(signinDTO.username, signinDTO.password, isPersistent: false, lockoutOnFailure: false);

        //    if (!result.Succeeded)
        //        return Unauthorized();


        //    var jwtToken = tokenGenerator.GenerateToken(Guid.NewGuid(), signinDTO.username, "Admin");

        //    var RefreshToken = tokenGenerator.GenerateRefreshToken();

        //    var userId = pesonsDbContext.Users.Where(s => s.UserName == signinDTO.username).Select(s => s.Id).FirstOrDefault();

        //    await pesonsDbContext.RefreshTokens.AddAsync(new RefreshToken
        //    {
        //        TokenHash = tokenGenerator.HashToken(RefreshToken),
        //        userId = userId,
        //        ExpiresAt = DateTime.UtcNow.AddDays(7)

        //    });

        //    await pesonsDbContext.SaveChangesAsync(new CancellationToken());

        //    Response.Cookies.Append("refreshToken", RefreshToken, new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        SameSite = SameSiteMode.Strict,
        //        Expires = DateTime.UtcNow.AddDays(7)

        //    });

        //    return Ok(new { jwtToken });
        //}


        [HttpPost("signin")]
        public async Task<IActionResult> Signin(SigninDTO signinDTO)
        {
            var result = await signInManager.PasswordSignInAsync(signinDTO.username, signinDTO.password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
                return Unauthorized();

            var user = await userManager.FindByNameAsync(signinDTO.username); // ✅ get real user
            if (user == null)
                return Unauthorized();

            var jwtToken = tokenGenerator.GenerateToken(user.Id, user.UserName!, "Admin"); // ✅ real ID, not random Guid

            var refreshToken = tokenGenerator.GenerateRefreshToken();

            await pesonsDbContext.RefreshTokens.AddAsync(new RefreshToken
            {
                TokenHash = tokenGenerator.HashToken(refreshToken),
                userId = user.Id, // ✅ matches JWT
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            await pesonsDbContext.SaveChangesAsync();

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new { jwtToken });
        }


        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshTheToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var hashedRefToken = tokenGenerator.HashToken(refreshToken);

            var storedRefToken = pesonsDbContext.RefreshTokens.FirstOrDefault(t => t.TokenHash == hashedRefToken);

            if (storedRefToken == null || !storedRefToken.IsActive)
                return Unauthorized();

            var user = await userManager.FindByIdAsync(storedRefToken.userId.ToString());

            if (user == null)
                return Unauthorized();

            var newRefreshToken = tokenGenerator.GenerateRefreshToken();
            storedRefToken.RevokedAt = DateTime.UtcNow;
            storedRefToken.RevokedByTokenHash = tokenGenerator.HashToken(newRefreshToken);

            await pesonsDbContext.RefreshTokens.AddAsync(new RefreshToken
            {
                TokenHash = storedRefToken.RevokedByTokenHash,
                userId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            await pesonsDbContext.SaveChangesAsync(new CancellationToken());

            var newAccessToken = tokenGenerator.GenerateToken(user.Id, user.UserName!, "Admin");

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new { newAccessToken });
        }




        [HttpGet("logout")]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return Ok();
        }
    }
}
