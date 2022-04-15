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
            var group = Group.NewInstance(chair);
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
                Name = x.Bylaws.Name,
                Minutes = x.Minutes
            };
        }

        public async Task<IEnumerable<string>> GetAvailableActions(Guid userId, Guid groupId)
        {
            var actor = await _database.GetPersonAsync(userId);
            var group = await _database.GetGroupAsync(groupId);
            var actions = group.GetAvailableActions(actor);
            return actions.Select(x => x.ToString());
        }
        
        public async Task<IEnumerable<string>> GetAvailableMotions(Guid userId, Guid groupId)
        {
            var actor = await _database.GetPersonAsync(userId);
            var group = await _database.GetGroupAsync(groupId);
            var actions = group.GetAvailableMotions(actor);
            return actions.Select(x => x.ToString());
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
            var group = await _database.GetGroupAsync(groupId);
            IMotion motion = new ElectChair(nominee, group);
            IAction action = new Move(motion);
            group.TakeAction(actor, action);
            await _database.UpdateGroupAsync(group);
        }

        public async Task MoveResolution(Guid userId, Guid groupId, string resolution)
        {
            var actor = await _database.GetPersonAsync(userId);
            var group = await _database.GetGroupAsync(groupId);
            var motion = new Resolve(resolution, group);
            var action = new Move(motion);
            group.TakeAction(actor, action);
            await _database.UpdateGroupAsync(group);
        }
        
        public async Task MoveChangeGroupName(Guid userId, Guid groupId, string groupName)
        {
            var actor = await _database.GetPersonAsync(userId);
            var group = await _database.GetGroupAsync(groupId);
            var motion = new ChangeOrgName(groupName, group);
            var action = new Move(motion);
            group.TakeAction(actor, action);
            await _database.UpdateGroupAsync(group);
        }
        
        public async Task MoveGrantMembership(Guid userId, Guid groupId, Guid nomineeId)
        {
            var actor = await _database.GetPersonAsync(userId);
            var nominee = await _database.GetPersonAsync(nomineeId);
            var group = await _database.GetGroupAsync(groupId);
            var motion = new GrantMembership(nominee, group);
            var action = new Move(motion);
            group.TakeAction(actor, action);
            await _database.UpdateGroupAsync(group);
        }

        public async Task<IEnumerable<string>> GetMinutes(Guid groupId)
        {
            var group = await _database.GetGroupAsync(groupId);
            return group.Minutes;
        }
    }
}