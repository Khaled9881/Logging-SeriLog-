using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revision_of_Data_Seeding.Models
{
    public class AuthorProfile
    {
        [Key, ForeignKey("Author")]
        public int AuthorProfileID { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public Author Author { get; set; }
    }
}
