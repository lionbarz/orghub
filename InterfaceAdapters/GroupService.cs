using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using InterfaceAdapters.Models;

namespace InterfaceAdapters
{
    public class GroupService
    {
        private readonly IDatabaseAccess _db;

        public GroupService(IDatabaseAccess database)
        {
            _db = database;
        }

        public async Task<UXGroup> AddGroupAsync(Guid chairUserId, string name, string mission, IEnumerable<string> memberEmails)
        {
            var chair = await _db.GetPersonAsync(chairUserId);
            var group = Group.NewInstance(chair, name, mission);
            
            foreach (var email in memberEmails)
            {
                Person person = null;
                
                // If nobody with this email exists already, add them so the
                // account can be claimed later when the person signs in.
                // TODO: Do this better? Have a "Member" object that can contain an ID or just an email? Is that better? So we don't confuse real existing people with "shadow" accounts?
                if (!await _db.TryGetPersonByEmailAsync(email, out person))
                {
                    var newPerson = new Person(null, email);
                    await _db.AddPersonAsync(newPerson);
                    person = newPerson;
                }
                
                group.AddMember(person);
            }
            
            await _db.AddGroupAsync(group);
            return ToUxGroup(group);
        }

        public async Task<IEnumerable<UXGroup>> GetGroupsAsync()
        {
            var groups = await _db.GetGroupsAsync();
            return groups.Select(ToUxGroup);
        }

        public async Task<UXGroup> GetGroupAsync(Guid id)
        {
            var group = await _db.GetGroupAsync(id);
            return ToUxGroup(group);
        }

        private static UXGroup ToUxGroup(Group x)
        {
            return new UXGroup()
            {
                Id = x.Id,
                Chair = PersonService.ToUxPerson(x.Chair),
                Members = x.Members.Select(PersonService.ToUxPerson),
                Resolutions = x.Resolutions,
                Name = x.Bylaws.Name
            };
        }
        
        public async Task AddMemberAsync(Guid groupId, Guid personId)
        {
            var person = await _db.GetPersonAsync(personId);
            var group = await _db.GetGroupAsync(groupId);
            group.AddMember(person);
            await _db.UpdateGroupAsync(group);
        }
    }
}