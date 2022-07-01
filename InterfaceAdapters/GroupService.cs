using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Motions;
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

        public async Task<UXGroup> AddGroupAsync(Guid chairUserId)
        {
            var chair = await _db.GetPersonAsync(chairUserId);
            var group = Group.NewInstance(chair);
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
                Chair = new UXPerson()
                {
                    Id = x.Chair.Id,
                    Name = x.Chair.Name
                },
                Members = x.Members.Select(m => new UXPerson()
                {
                    Id = m.Id,
                    Name = m.Name
                }),
                Resolutions = x.Resolutions,
                Name = x.Bylaws.Name,
                Minutes = x.Minutes,
                State = x.State.GetDescription()
            };
        }

        public async Task<IEnumerable<string>> GetMinutes(Guid groupId)
        {
            var group = await _db.GetGroupAsync(groupId);
            return group.Minutes;
        }
        
        public async Task AddMemberAsync(Guid groupId, Guid personId)
        {
            var person = await _db.GetPersonAsync(personId);
            var group = await _db.GetGroupAsync(groupId);
            group.AddMember(person);
            await _db.UpdateGroupAsync(group);
        }
        
        public async Task CallToOrder(Guid groupId, Guid personId)
        {
            var group = await _db.GetGroupAsync(groupId);
            var person = await _db.GetPersonAsync(personId);
            var personRole = group.CreatePersonRole(person);
            group.State.CallMeetingToOrder(personRole);
            await _db.UpdateGroupAsync(group);
        }

        public async Task<IEnumerable<string>> GetAvailableActions(Guid groupId, Guid personId)
        {
            var group = await _db.GetGroupAsync(groupId);
            var person = await _db.GetPersonAsync(personId);
            var personRole = group.CreatePersonRole(person);
            var actionSupports = group.State.GetActionSupportForPerson(personRole);
            return actionSupports.Where(x => x.IsSupported).Select(x => x.Action.ToString());
        }

        public async Task MarkAttendance(Guid groupId, Guid personId)
        {
            var group = await _db.GetGroupAsync(groupId);
            var person = await _db.GetPersonAsync(personId);
            group.MarkAttendance(person);
            await _db.UpdateGroupAsync(group);
        }

        public async Task MoveResolution(Guid groupId, Guid personId, string text)
        {
            var group = await _db.GetGroupAsync(groupId);
            var person = await _db.GetPersonAsync(personId);
            var personRole = group.CreatePersonRole(person);
            group.State.MoveMainMotion(personRole, new Resolve(text, group));
            await _db.UpdateGroupAsync(group);
        }

        public async Task DeclareTimeExpired(Guid groupId, Guid personId)
        {
            var group = await _db.GetGroupAsync(groupId);
            var person = await _db.GetPersonAsync(personId);
            var personRole = group.CreatePersonRole(person);
            group.State.DeclareTimeExpired(personRole);
            await _db.UpdateGroupAsync(group);
        }
        
        public async Task Second(Guid groupId, Guid personId)
        {
            var group = await _db.GetGroupAsync(groupId);
            var person = await _db.GetPersonAsync(personId);
            var personRole = group.CreatePersonRole(person);
            group.State.Second(personRole);
            await _db.UpdateGroupAsync(group);
        }

        public async Task Speak(Guid groupId, Guid personId)
        {
            var group = await _db.GetGroupAsync(groupId);
            var person = await _db.GetPersonAsync(personId);
            var personRole = group.CreatePersonRole(person);
            group.State.Speak(personRole);
            await _db.UpdateGroupAsync(group);
        }
        
        public async Task Vote(Guid groupId, Guid personId, VoteType voteType)
        {
            var group = await _db.GetGroupAsync(groupId);
            var person = await _db.GetPersonAsync(personId);
            var personRole = group.CreatePersonRole(person);
            group.State.Vote(personRole, voteType);
            await _db.UpdateGroupAsync(group);
        }
        
        public async Task Yield(Guid groupId, Guid personId)
        {
            var group = await _db.GetGroupAsync(groupId);
            var person = await _db.GetPersonAsync(personId);
            var personRole = group.CreatePersonRole(person);
            group.State.Yield(personRole);
            await _db.UpdateGroupAsync(group);
        }

        public async Task MoveToAdjourn(Guid groupId, Guid personId)
        {
            var group = await _db.GetGroupAsync(groupId);
            var person = await _db.GetPersonAsync(personId);
            var personRole = group.CreatePersonRole(person);
            group.State.MoveToAdjourn(personRole);
            await _db.UpdateGroupAsync(group);
        }
    }
}