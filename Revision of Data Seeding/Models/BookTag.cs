namespace Revision_of_Data_Seeding.Models
{
    public class BookTag
    {
        public Guid BookID { get; set; }
        public Book Book { get; set; }

        public int TagID { get; set; }
        public Tag Tag { get; set; }

        public DateTime TaggedDate { get; set; }
    }
}
