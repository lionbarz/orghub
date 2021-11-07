using System.Threading.Tasks;
using Core;

namespace InterfaceAdapters
{
    public class PersonService
    {
        private readonly IDatabaseAccess _database;
        
        public PersonService(IDatabaseAccess database)
        {
            _database = database;
        }

        public async Task AddPerson(Person person)
        {
            await _database.AddPersonAsync(person);
        }
    }
}