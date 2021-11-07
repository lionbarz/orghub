using System;
using System.Threading.Tasks;
using Core;

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
    }
}