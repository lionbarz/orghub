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

        public async Task<UxPerson> AddPerson(string name, string email)
        {
            // Check if a person with the same email already exists.
            if (await _database.TryGetPersonByEmailAsync(email, out Person existingPerson))
            {
                if (string.IsNullOrEmpty(existingPerson.Name))
                {
                    // Update the name in the database.
                    existingPerson.Name = name;
                    await _database.AddPersonAsync(existingPerson);
                }
                
                return ToUxPerson(existingPerson);
            }
            
            var person = new Person(name, email);
            await _database.AddPersonAsync(person);
            return ToUxPerson(person);
        }

        public async Task<UxPerson> GetPersonAsync(Guid id)
        {
            var person = await _database.GetPersonAsync(id);
            return ToUxPerson(person);
        }

        public async Task<IEnumerable<UxPerson>> GetPersons()
        {
            var people = await _database.GetPersonsAsync();
            return people.Select(ToUxPerson);
        }

        public static UxPerson ToUxPerson(Person person)
        {
            return new UxPerson()
            {
                Id = person.Id,
                Name = person.Name,
                // TODO: Don't send everyone's emails to the frontend.
                Email = person.Email
            };
        }
    }
}