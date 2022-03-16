using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Actions;
using Core.Motions;
using InterfaceAdapters.Models;

namespace InterfaceAdapters
{
    public class GroupService
    {
        private readonly IDatabaseAccess _database;

        public GroupService(IDatabaseAccess database)
        {
            _database = database;
        }

        public async Task<UXGroup> AddGroupAsync(Guid userId)
        {
            var chair = await _database.GetPersonAsync(userId);
            var group = Group.MassMeeting(chair);
            await _database.AddGroupAsync(group);
            return ToUxGroup(group);
        }

        public async Task<IEnumerable<UXGroup>> GetGroupsAsync()
        {
            var groups = await _database.GetGroupsAsync();
            return groups.Select(ToUxGroup);
        }

        public async Task<UXGroup> GetGroupAsync(Guid id)
        {
            var group = await _database.GetGroupAsync(id);
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
                State = x.GetState().GetDescription(),
                Resolutions = x.Resolutions,
                Name = x.Bylaws.Name
            };
        }

        public async Task ActAsync(Guid userId, Guid groupId, IAction action)
        {
            var actor = await _database.GetPersonAsync(userId);
            var group = await _database.GetGroupAsync(groupId);
            group.TakeAction(actor, action);
            await _database.UpdateGroupAsync(group);
        }

        public async Task ElectChair(Guid userId, Guid groupId, string nomineeName)
        {
            var actor = await _database.GetPersonAsync(userId);
            Person nominee = new Person(nomineeName);
            IMotion motion = new ElectChair(nominee);
            IAction action = new Move(motion);
            var group = await _database.GetGroupAsync(groupId);
            group.TakeAction(actor, action);
            await _database.UpdateGroupAsync(group);
        }

        public async Task MoveResolution(Guid userId, Guid groupId, string resolution)
        {
            var actor = await _database.GetPersonAsync(userId);
            var motion = new Resolve(resolution);
            var group = await _database.GetGroupAsync(groupId);
            var action = new Move(motion);
            group.TakeAction(actor, action);
            await _database.UpdateGroupAsync(group);
        }
        
        public async Task MoveChangeGroupName(Guid userId, Guid groupId, string groupName)
        {
            var actor = await _database.GetPersonAsync(userId);
            var motion = new ChangeOrgName(groupName);
            var group = await _database.GetGroupAsync(groupId);
            var action = new Move(motion);
            group.TakeAction(actor, action);
            await _database.UpdateGroupAsync(group);
        }
    }
}