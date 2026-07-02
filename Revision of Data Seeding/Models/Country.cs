using System.ComponentModel.DataAnnotations;

namespace Revision_of_Data_Seeding.Models
{
    public class Country
    {
        [Key]
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }
    }
}
