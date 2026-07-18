using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Revision_of_Data_Seeding.Models;
using Serilog;
using SerilogTimings;

namespace Revision_of_Data_Seeding.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly PesonsDbContext pesonsDbContext;
        private readonly IDiagnosticContext diagnosticContext;

        public WeatherForecastController(PesonsDbContext _pesonsDbContext, IDiagnosticContext diagnosticContext)
        {
            pesonsDbContext = _pesonsDbContext;
            this.diagnosticContext = diagnosticContext;
        }


        [HttpGet(Name = "getallpersons")]
        //[Authorize(Roles = "Admin")]
        [Authorize("NotAuthorized")]
        public async Task<ActionResult> Get()
        {
            using (Operation.Time("Time for DB to return all persons"))
            {
                var persons = pesonsDbContext.Persons.FromSqlRaw("EXEC GETALLPERSONS").ToList();
                diagnosticContext.Set("Persons", persons);
                return Ok(persons);

            }

            //return Ok(pesonsDbContext.Persons.FromSqlRaw("EXEC GETPERSONBYID @PersonID = {0}", Guid.Parse("a3b9833b-8a4d-43e9-8690-61e08df81a9a")));

            //Country country = new Country()
            //{
            //    CountryID = Guid.Parse("a3b9833b-8a4d-43e9-8690-61e08df81a9a"),
            //    CountryName = "USA"
            //};

            //return Ok(await pesonsDbContext.Database.ExecuteSqlRawAsync("EXEC INSERTCOUNTRY @CountryID = {0}, @CountryName = {1}", country.CountryID, country.CountryName));
        }
    }
}
