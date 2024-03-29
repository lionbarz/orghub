﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using InterfaceAdapters.Models;

namespace InterfaceAdapters
{
    public interface IDatabaseAccess
    {
        Task AddGroupAsync(Group group);
        Task UpdateGroupAsync(Group group);
        Task<Group> GetGroupAsync(Guid groupId);
        Task<IEnumerable<Group>> GetGroupsAsync();
        
        Task AddPersonAsync(Person person);
        Task<Person> GetPersonAsync(Guid personId);
        Task<bool> TryGetPersonByEmailAsync(string email, out Person person);
        Task<IEnumerable<Person>> GetPersonsAsync();

        Task AddMeetingAsync(Meeting meeting);
        Task<IEnumerable<Meeting>> ListMeetingsAsync();
        Task UpdateMeetingAsync(Meeting meeting);
        Task<Meeting> GetMeetingAsync(Guid id);
    }
}