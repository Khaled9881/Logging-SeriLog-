using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revision_of_Data_Seeding.Models
{

    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered,
        Cancelled
    }

    public class Author
    {
        [Key]
        public int AuthorID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column("EmailAddress")]
        [MaxLength(150)]
        public string Email { get; set; }

        // One-to-one: Author has one AuthorProfile
        public AuthorProfile Profile { get; set; }

        // One-to-many: Author has many Books
        public List<Book> Books { get; set; } = new();
    }
}
