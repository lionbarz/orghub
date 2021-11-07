using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace InterfaceAdapters
{
    public class MotionService
    {
        private readonly IDatabaseAccess _database;

        public MotionService(IDatabaseAccess database)
        {
            _database = database;
        }
        
        public async Task MakeMotionAsync(Guid groupId, MotionToCreateOrganization motion)
        {
            var group = await _database.GetGroupAsync(groupId);
            group.Move(motion);
            await _database.UpdateGroupAsync(group);
        }
    }
}