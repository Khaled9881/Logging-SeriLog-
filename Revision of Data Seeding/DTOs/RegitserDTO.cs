using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Revision_of_Data_Seeding.DTOs
{
    public class RegitserDTO
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

    }
}
