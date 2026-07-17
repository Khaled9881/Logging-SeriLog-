using System.ComponentModel.DataAnnotations;

namespace Revision_of_Data_Seeding.DTOs
{
    public class SigninDTO
    {
        [Required]
        public string username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string password { get; set; }
    }
}
