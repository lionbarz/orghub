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

        public Task AddGroupAsync(Group group)
        {
            if (group.Bylaws != null)
            {
                throw new Exception(
                    "Cannot add a group with bylaws. Bylaws must be approved by a vote which will create a group.");
            }

            _database.AddGroupAsync(group);
            return Task.CompletedTask;
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
                Resolutions = x.Resolutions
            };
        }

        public async Task ActAsync(string authToken, Guid groupId, IAction action)
        {
            var actor = await _database.GetPersonAsync(MeetingService._userId); // TODO: Login
            var group = await _database.GetGroupAsync(groupId);
            group.TakeAction(actor, action);
            await _database.UpdateGroupAsync(group);
        }

        public async Task ElectChair(Guid groupId, string nomineeName)
        {
            var actor = await _database.GetPersonAsync(MeetingService._userId); // TODO: Login
            Person nominee = new Person(nomineeName);
            IMotion motion = new ElectChair(nominee);
            IAction action = new Move(motion);
            var group = await _database.GetGroupAsync(groupId);
            group.TakeAction(actor, action);
            await _database.UpdateGroupAsync(group);
        }

        public async Task MoveResolution(Guid groupId, string resolution)
        {
            var actor = await _database.GetPersonAsync(MeetingService._userId); // TODO: Login
            var motion = new Resolve(resolution);
            var group = await _database.GetGroupAsync(groupId);
            var action = new Move(motion);
            group.TakeAction(actor, action);
            await _database.UpdateGroupAsync(group);
        }
    }
}