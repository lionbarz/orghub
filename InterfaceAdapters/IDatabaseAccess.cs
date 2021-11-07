using System;
using System.Threading.Tasks;
using Core;

namespace InterfaceAdapters
{
    public interface IDatabaseAccess
    {
        Task AddGroupAsync(Group group);
        Task UpdateGroupAsync(Group group);
        Task<Group> GetGroupAsync(Guid groupId);
        Task AddPersonAsync(Person person);
        Task<Person> GetPersonAsync(Guid personId);
    }
}