using Microsoft.AspNetCore.Identity;

namespace Revision_of_Data_Seeding.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public List<RefreshToken> refreshTokens { get; set; } = new();
    }
}
