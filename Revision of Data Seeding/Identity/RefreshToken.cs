using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revision_of_Data_Seeding.Identity
{
    public class RefreshToken
    {
        [Key]
        public int id { get; set; }
        public string TokenHash { get; set; } = default!;
        [ForeignKey("applicationUser")]
        public Guid userId { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public string? RevokedByTokenHash { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt != null;
        public bool IsActive => !IsExpired && !IsRevoked;

        public ApplicationUser applicationUser { get; set; }

    }
}
