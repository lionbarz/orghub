using System;
using System.Threading.Tasks;

namespace Core.Motions
{
    /// <summary>
    /// Set the chair of a group to a specific person.
    /// </summary>
    public class ElectChair : IGroupModifyingMotion
    {
        private readonly Person _nominee;

        public ElectChair(Person nominee)
        {
            _nominee = nominee;
        }
        
        public string GetText()
        {
            return $"Elect {_nominee.Name} as the chair of the group.";
        }

        public Task TakeActionAsync(IGroupModifier groupModifier)
        {
            groupModifier.SetChair(_nominee);
            return Task.CompletedTask;
        }
    }
}