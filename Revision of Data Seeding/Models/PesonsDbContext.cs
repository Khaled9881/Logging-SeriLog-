using Microsoft.EntityFrameworkCore;

namespace Revision_of_Data_Seeding.Models
{
    public class PesonsDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Country> Countries { get; set; }

        public PesonsDbContext(DbContextOptions<PesonsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().ToTable(nameof(Person));
            modelBuilder.Entity<Country>().ToTable(nameof(Country));

            modelBuilder.Entity<Country>().HasData(
                new Country { CountryID = Guid.NewGuid(), CountryName = "USA" },
                new Country { CountryID = Guid.NewGuid(), CountryName = "Canada" },
                new Country { CountryID = Guid.NewGuid(), CountryName = "UK" }
            );

            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(PesonsDbContext).Assembly);

            var personsJson = System.IO.File.ReadAllText("Persons.json");

            var persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (var person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }

            modelBuilder.Entity<Person>().HasData(new PersonData().GetPersons());
        }
    }
}
