using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using InterfaceAdapters;

namespace Databases
{
    /// <summary>
    /// In memory data store.
    /// </summary>
    public class InMemoryDatabase : IDatabaseAccess
    {
        private readonly Dictionary<Guid, Group> _groupDict = new();
        private readonly Dictionary<Guid, Person> _personDict = new();

        public Task AddGroupAsync(Group group)
        {
            _groupDict[group.Id] = group;
            return Task.CompletedTask;
        }

        public async Task UpdateGroupAsync(Group group)
        {
            _groupDict[group.Id] = group;
        }

        public Task<Group> GetGroupAsync(Guid groupId)
        {
            return Task.FromResult(_groupDict[groupId]);
        }

        public Task AddPersonAsync(Person person)
        {
            _personDict[person.Id] = person;
            return Task.CompletedTask;
        }

        public Task<Person> GetPersonAsync(Guid personId)
        {
            return Task.FromResult(_personDict[personId]);
        }
    }
}