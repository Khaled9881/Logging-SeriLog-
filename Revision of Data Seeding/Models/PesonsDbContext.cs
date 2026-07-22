using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Revision_of_Data_Seeding.Identity;

namespace Revision_of_Data_Seeding.Models
{
    public class PesonsDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public PesonsDbContext(DbContextOptions<PesonsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().ToTable(nameof(Person));
            modelBuilder.Entity<Country>().ToTable(nameof(Country));

            modelBuilder.Entity<Country>().HasData(
    new Country { CountryID = Guid.Parse("BE699755-747B-41A6-8072-576151C53AB9"), CountryName = "USA", TIN = "ABC12345" },
    new Country { CountryID = Guid.Parse("40C3ACB3-1855-4E9D-AAD0-62F8425C37AE"), CountryName = "Canada", TIN = "ABC12345" },
    new Country { CountryID = Guid.Parse("4009667F-7000-4069-B31D-B153D0325E26"), CountryName = "UK", TIN = "ABC12345" }
);

            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(PesonsDbContext).Assembly);

            var personsJson = System.IO.File.ReadAllText("Persons.json");

            var persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (var person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }

            //modelBuilder.Entity<Person>().HasData(new PersonData().GetPersons());


            modelBuilder.Entity<Country>().Property(x => x.TIN)
                .HasColumnName("TaxIdNumber")
                .HasColumnType("varchar(8)")
                .HasDefaultValue("ABC12345");


            // Exercise
            // Author

            modelBuilder.Entity<Author>().ToTable("Author");
            modelBuilder.Entity<Author>().HasKey(a => a.AuthorID);
            modelBuilder.Entity<Author>().Property(a => a.AuthorID);


            modelBuilder.Entity<Author>().Property(a => a.Name).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Author>().Property(a => a.Email).HasColumnName("EmailAddress");

            modelBuilder.Entity<Author>().HasOne(a => a.Profile).WithOne(a => a.Author).HasForeignKey<AuthorProfile>(ap => ap.AuthorProfileID);


            modelBuilder.Entity<Author>().HasMany(a => a.Books).WithOne(a => a.Author).HasForeignKey(a => a.AuthorID).OnDelete(DeleteBehavior.Cascade);


            // AuthorProfile

            modelBuilder.Entity<AuthorProfile>().ToTable("AuthorProfile");
            modelBuilder.Entity<AuthorProfile>().HasKey(s => s.AuthorProfileID);
            modelBuilder.Entity<AuthorProfile>().Property(s => s.Bio).HasMaxLength(500);

            // Book

            modelBuilder.Entity<Book>().ToTable("Book");

            modelBuilder.Entity<Book>().HasKey(s => s.BookID);
            modelBuilder.Entity<Book>().Property(s => s.BookID);

            modelBuilder.Entity<Book>().Property(s => s.Title).IsRequired().HasMaxLength(200);

            modelBuilder.Entity<Book>().Property(s => s.Price).HasColumnType("decimal(8,2)");

            modelBuilder.Entity<Book>().HasQueryFilter(s => !s.IsDeleted);

            modelBuilder.Entity<Book>().Property(s => s.RowVersion).IsRowVersion();


            // Tag 

            modelBuilder.Entity<Tag>().HasKey(s => s.TagID);

            modelBuilder.Entity<Tag>().Property(s => s.Name).IsRequired().HasMaxLength(30);



            modelBuilder.Entity<BookTag>()
                .HasKey(s => new
                {
                    s.BookID,
                    s.TagID
                });

            modelBuilder.Entity<BookTag>()
                .HasOne(b => b.Book)
                .WithMany(b => b.BookTags)
                .HasForeignKey(b => b.BookID);


            modelBuilder.Entity<BookTag>()
               .HasOne(b => b.Tag)
               .WithMany(b => b.BookTags)
               .HasForeignKey(b => b.TagID);


            //modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            //{

            //});


            modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.applicationUser)
            .WithMany(u => u.refreshTokens)
            .HasForeignKey(rt => rt.userId)
            .OnDelete(DeleteBehavior.Cascade); // delete tokens when user is deleted

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.TokenHash)
                .IsUnique();



        }
    }
}
