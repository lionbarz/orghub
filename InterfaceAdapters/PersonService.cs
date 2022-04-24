using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using InterfaceAdapters.Models;

namespace InterfaceAdapters
{
    public class PersonService
    {
        private readonly IDatabaseAccess _database;
        
        public PersonService(IDatabaseAccess database)
        {
            _database = database;
        }

        public async Task<UXPerson> AddPerson(string name)
        {
            var person = new Person(name);
            await _database.AddPersonAsync(person);
            return ToUXPerson(person);
        }

        public async Task<UXPerson> GetPersonAsync(Guid id)
        {
            var person = await _database.GetPersonAsync(id);
            return ToUXPerson(person);
        }

        public async Task<IEnumerable<UXPerson>> GetPersons()
        {
            var people = await _database.GetPersonsAsync();
            return people.Select(ToUXPerson);
        }

        private UXPerson ToUXPerson(Person person)
        {
            return new UXPerson()
            {
                Id = person.Id,
                Name = person.Name
            };
        }
    }
}