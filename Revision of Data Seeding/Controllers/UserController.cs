using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Revision_of_Data_Seeding.DTOs;
using Revision_of_Data_Seeding.Identity;

namespace Revision_of_Data_Seeding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
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
                return Created();
            }

            return BadRequest(identityResult.Errors);
        }
    }
}
