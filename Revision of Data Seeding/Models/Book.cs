using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revision_of_Data_Seeding.Models
{
    public class Book
    {
        [Key]
        public Guid BookID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey("Author")]
        public int AuthorID { get; set; }
        public Author Author { get; set; }

        // Many-to-many with Tag (needs a join entity for extra column: TaggedDate)
        public List<BookTag> BookTags { get; set; } = new();

        public OrderStatus Status { get; set; }
    }
}
