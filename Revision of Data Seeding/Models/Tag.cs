using System.ComponentModel.DataAnnotations;

namespace Revision_of_Data_Seeding.Models
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public List<BookTag> BookTags { get; set; } = new();
    }
}
