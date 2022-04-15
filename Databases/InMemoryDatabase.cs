using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using InterfaceAdapters;
using InterfaceAdapters.Models;

namespace Databases
{
    /// <summary>
    /// In memory data store.
    /// </summary>
    public class InMemoryDatabase : IDatabaseAccess
    {
        private readonly Dictionary<Guid, Group> _groupDict = new();
        private readonly Dictionary<Guid, Person> _personDict = new();
        private readonly Dictionary<Guid, Meeting> _meetingDict = new();

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

        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            return _groupDict.Values;
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

        public async Task<IEnumerable<Person>> GetPersonsAsync()
        {
            return _personDict.Values;
        }

        public Task AddMeetingAsync(Meeting meeting)
        {
            _meetingDict[meeting.Id] = meeting;
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Meeting>> ListMeetingsAsync()
        {
            return _meetingDict.Values;
        }

        public Task UpdateMeetingAsync(Meeting meeting)
        {
            _meetingDict[meeting.Id] = meeting;
            return Task.CompletedTask;
        }

        public async Task<Meeting> GetMeetingAsync(Guid id)
        {
            return _meetingDict[id];
        }
    }
}