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


        [HttpPost("signin")]
        public async Task<IActionResult> Signin(SigninDTO signinDTO)
        {

            //var user = await userManager.FindByEmailAsync(signinDTO.username);
            //if (user == null) 
            //    return Unauthorized();


            var result = await signInManager.PasswordSignInAsync(signinDTO.username, signinDTO.password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
                return Unauthorized();

            //var userId = userManager.GetUserId;


            var jwtToken = tokenGenerator.GenerateToken(Guid.NewGuid(), signinDTO.username, "Admin");
            return Ok(new { jwtToken });
        }


        [HttpGet("logout")]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return Ok();
        }
    }
}
