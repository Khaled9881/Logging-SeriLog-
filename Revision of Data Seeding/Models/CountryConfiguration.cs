using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Revision_of_Data_Seeding.Models
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new Country { CountryID = Guid.NewGuid(), CountryName = "United States" },
                new Country { CountryID = Guid.NewGuid(), CountryName = "Canada" },
                new Country { CountryID = Guid.NewGuid(), CountryName = "Mexico" }
            );

        }
    }
}
