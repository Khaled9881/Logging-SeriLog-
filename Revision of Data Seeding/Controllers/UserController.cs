using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Revision_of_Data_Seeding.DTOs;
using Revision_of_Data_Seeding.Identity;
using Revision_of_Data_Seeding.Models;

namespace Revision_of_Data_Seeding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly PesonsDbContext pesonsDbContext;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, PesonsDbContext pesonsDbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.pesonsDbContext = pesonsDbContext;
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

            return Ok();
        }
    }
}
