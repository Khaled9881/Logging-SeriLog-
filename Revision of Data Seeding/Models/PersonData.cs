namespace Revision_of_Data_Seeding.Models
{
    public class PersonData
    {
        public List<Person> Persons { get; set; }

        public PersonData()
        {
            Persons = new List<Person>()
            {
                new Person { PersonID = Guid.NewGuid(), PersonName = "John Doe", Email = "", DateOfBirth = new DateTime(1990, 1, 1), Gender = "Male", CountryID = Guid.Empty }
            };
        }

        public List<Person> GetPersons()
        {
            return Persons;
        }
    }
}
